using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core;
using Assets.Scripts.Entity.Effects;
using Assets.Scripts.Entity.Stat;
using Assets.Scripts.GameSession.Events;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Entity
{
    public class Entity : MonoBehaviour
    {
        public bool IsOnScreen { get; private set; }
        public bool Alive => !LifePoints.IsEmpty;
        public const int DeathLifePointsThreshold = 0;
        public const int MinimalDamageTaken = 1;
        public Resource LifePoints { get; private set; }
        public Resource KillsCount { get; private set; }
        public Stat.Stat Size { get; private set; }
        public Stat.Stat MaximumLifePoints { get; private set; }
        public Stat.Stat LifeRegenerationPerSecond { get; private set; }
        public Stat.Stat KnockbackPower { get; private set; }
        public Stat.Stat Damage { get; private set; }

        public SpriteRenderer SpriteRenderer { get; private set; }
        public GameTimeScheduler GameTimeScheduler { get; private set; }
        public StatFactory StatFactory { get; private set; }

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
            effect.StacksCount.Set(1);
            effects.Add(effect);
            effect.Attach(this);
            effect.Subscribe(this);
        }

        public void RemoveEffectStack(IEffect effect)
        {
            foreach (var myEffect in effects.Where(myEffect => myEffect.Name == effect.Name))
            {
                myEffect.StacksCount.Decrease();
                if (!myEffect.StacksCount.IsEmpty) return;
                RemoveEffect(effect);
            }
        }

        public void RemoveEffect(IEffect effect)
        {
            var isContain = false;

            foreach (var myEffect in effects.Where(myEffect => myEffect.Name == effect.Name))
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
            TryGetComponent<SpriteRenderer>(out SpriteRenderer sR);
            SpriteRenderer = sR;

            StatFactory = Camera.main.GetComponent<StatFactory>();

            Size = StatFactory.GetStat(settings.size);
            MaximumLifePoints = StatFactory.GetStat(settings.maximumLife);
            LifeRegenerationPerSecond = StatFactory.GetStat(settings.lifeRegenerationInSecond);
            KnockbackPower = StatFactory.GetStat(settings.knockbackPower);
            Damage = StatFactory.GetStat(settings.damage);
        
            transform.localScale = new Vector3(Size.Value, Size.Value, 1);

            LifePoints = new Resource(DeathLifePointsThreshold, MaximumLifePoints, LifeRegenerationPerSecond);
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
            var stat = (Stat.Stat)EventHelper.GetPropByPath(this, statPath);
            stat?.AddModifier(statModifier);
        }

        public void RemoveStatModifier(StatModifier statModifier, string statName)
        {
            var stat = (Stat.Stat)EventHelper.GetPropByPath(this, statName);
            stat?.RemoveModifier(statModifier);
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