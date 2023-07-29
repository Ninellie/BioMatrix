using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core;
using Assets.Scripts.EntityComponents.Effects;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.GameSession.Events;
using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    public class Entity : MonoBehaviour
    {
        public bool IsOnScreen { get; private set; }
        public bool Alive => !LifePoints.IsEmpty;
        public const int DeathLifePointsThreshold = 0;
        public const int MinimalDamageTaken = 1;
        public RecoverableResource LifePoints { get; private set; }
        public Resource KillsCount { get; private set; }
        public Stat Size { get; private set; }
        public Stat MaximumLifePoints { get; private set; }
        public Stat LifeRegenerationPerSecond { get; private set; }
        public Stat KnockbackPower { get; private set; }
        public Stat Damage { get; private set; }

        public SpriteRenderer SpriteRenderer { get; private set; }
        public GameTimeScheduler GameTimeScheduler { get; private set; }

        public readonly List<IEffect> effects = new();

        public void AddEffectStack(IEffect effect)
        {
            foreach (var myEffect in effects.Where(myEffect => myEffect.Name == effect.Name))
            {
                if (myEffect.IsStacking)
                {
                    var isCannotIncreaseStack = myEffect.StacksCount.IsFull;
                    if (isCannotIncreaseStack) return;

                    myEffect.StacksCount.Increase();

                    if (myEffect.IsStackSeparateDuration)
                    {
                        GameTimeScheduler.Schedule(() => RemoveEffectStack(effect), effect.Duration.Value);
                        return; 
                    }
                }

                if (!myEffect.IsTemporal) return;

                if (myEffect.IsDurationUpdates)
                {
                    var newTime = Time.timeSinceLevelLoad + myEffect.Duration.Value;
                    GameTimeScheduler.UpdateTime(myEffect.Identifier, newTime);
                }

                if (myEffect.IsDurationStacks)
                {
                    GameTimeScheduler.Prolong(myEffect.Identifier, myEffect.Duration.Value);
                }
                return;
            }
            AddEffect(effect);
        }

        public void AddEffect(IEffect effect)
        {
            if (effect.IsTemporal)
            {
                if (!effect.IsStackSeparateDuration)
                {
                    effect.Identifier = GameTimeScheduler.Schedule(() => RemoveEffect(effect), effect.Duration.Value);
                }
                else
                {
                    effect.Identifier = GameTimeScheduler.Schedule(() => RemoveEffectStack(effect), effect.Duration.Value);
                }
            }
            effect.StacksCount.Empty();
            effect.StacksCount.Increase();
            effects.Add(effect);
            effect.Attach(this);
            effect.Subscribe(this);
        }

        public void RemoveEffectStack(IEffect effect)
        {
            var effectsNameToRemoveStack = new List<string>();
            var effectsNameToRemove = new List<string>();
            var effectsToRemove = new List<IEffect>();

            foreach (var myEffect in effects.Where(e => e.Name == effect.Name))
            {
                effectsNameToRemoveStack.Add(myEffect.Name);
            }

            foreach (var effectName in effectsNameToRemoveStack)
            {
                foreach (var myEffect in effects.Where(e => e.Name == effectName))
                {
                    myEffect.StacksCount.Decrease();
                    if (!myEffect.StacksCount.IsEmpty)
                        continue;
                    effectsNameToRemove.Add(effect.Name);
                }
            }
            
            foreach (var effectName in effectsNameToRemove)
            {
                foreach (var myEffect in effects.Where(e => e.Name == effectName))
                {
                    effectsToRemove.Add(myEffect);
                }
            }

            foreach (var effectToRemove in effectsToRemove)
            {
                effectToRemove.Unsubscribe(this);
                effectToRemove.Detach();
                effects.Remove(effectToRemove);
            }
        }

        public void RemoveEffect(IEffect effect)
        {
            var isContain = false;

            foreach (var myEffect in effects.Where(e => e.Name == effect.Name))
            {
                myEffect.StacksCount.Empty();
                isContain = true;
            }

            if (!isContain) return;
        
            effect.Unsubscribe(this);
            effect.Detach();
            effects.Remove(effect);
        }

        private void OnEnable() => BaseOnEnable();
    
        private void OnDisable() => BaseOnDisable();
    
        private void Update() => BaseUpdate();
    
        protected void BaseAwake(EntityStatsSettings settings)
        {
            Debug.Log($"{gameObject.name} Entity Awake");
            GameTimeScheduler = Camera.main.GetComponent<GameTimeScheduler>();
            TryGetComponent(out SpriteRenderer sR);
            SpriteRenderer = sR;

            Size = StatFactory.GetStat(settings.size);
            MaximumLifePoints = StatFactory.GetStat(settings.maximumLife);
            LifeRegenerationPerSecond = StatFactory.GetStat(settings.lifeRegenerationInSecond);
            KnockbackPower = StatFactory.GetStat(settings.knockbackPower);
            Damage = StatFactory.GetStat(settings.damage);
        
            transform.localScale = new Vector3(Size.Value, Size.Value, 1);

            LifePoints = new RecoverableResource(DeathLifePointsThreshold, MaximumLifePoints, LifeRegenerationPerSecond);
            LifePoints.Fill();
            KillsCount = new Resource();
        }
    
        protected virtual void BaseOnEnable()
        {
            Size.ValueChangedEvent += ChangeCurrentSize;
            LifePoints.EmptyEvent += Death;
        }
    
        protected virtual void BaseOnDisable()
        {
            Size.ValueChangedEvent -= ChangeCurrentSize;
            LifePoints.EmptyEvent -= Death;
        }
    
        protected virtual void BaseUpdate()
        {
            if (Time.timeScale == 0) return;
            LifePoints.TimeToRecover += Time.deltaTime;
            if (SpriteRenderer is null) return;
            IsOnScreen = CheckVisibilityOnCamera();
        }
        
        public virtual void TakeDamage(float amount)
        {
            LifePoints.Decrease((int)amount);
            Debug.Log("Damage is taken " + gameObject.name);
        }

        public virtual void RestoreLifePoints()
        {
            LifePoints.Fill();
        }

        public void AddStatModifier(StatModifier statModifier, string statPath)
        {
            var stat = (Stat)EventHelper.GetPropByPath(this, statPath);
            stat?.AddModifier(statModifier);
        }

        public void RemoveStatModifier(StatModifier statModifier, string statName)
        {
            var stat = (Stat)EventHelper.GetPropByPath(this, statName);
            stat?.RemoveModifier(statModifier);
        }
        
        public void IncreaseResourceValue(int value, string resourcePath)
        {
            var resource = (Resource)EventHelper.GetPropByPath(this, resourcePath);
            resource?.Increase(value);
        }

        public void DecreaseResourceValue(int value, string resourcePath)
        {
            var resource = (Resource)EventHelper.GetPropByPath(this, resourcePath);
            resource?.Decrease(value);
        }

        protected virtual void Death()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        protected virtual void ChangeCurrentSize()
        {
            transform.localScale = new Vector3(Size.Value, Size.Value, 1);
        }

        private bool CheckVisibilityOnCamera()
        {
            var onScreen = SpriteRenderer.isVisible;
            return onScreen;
        }
    }
}