using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Effects
{
    [CreateAssetMenu]
    public class EffectsRepositoryPreset : ScriptableObject
    {
        [SerializeReference]
        private List<IEffect> _effects;

        public List<IEffect> GetEffects()
        {
            return Instantiate(this)._effects;
        }

        public IEffect GetEffectByName(string effectName)
        {
            return Instantiate(this)._effects.FirstOrDefault(e => e.Name.Equals(effectName));
            //return _effects.FirstOrDefault(e => e.Name.Equals(effectName));
        }

        [ContextMenu(nameof(AddNewStackableStatModifierEffect))]
        private void AddNewStackableStatModifierEffect()
        {
            _effects.Add(new StackableStatModifierEffect());
        }

        [ContextMenu(nameof(AddNewStackableTemporaryStatModifierEffect))]
        private void AddNewStackableTemporaryStatModifierEffect()
        {
            _effects.Add(new StackableTemporaryStatModifierEffect());
        }

        [ContextMenu(nameof(AddNewRespondingEffectAdderEffect))]
        private void AddNewRespondingEffectAdderEffect()
        {
            _effects.Add(new RespondingEffectAdderEffect());
        }

        [ContextMenu(nameof(AddNewResourceIncreaserEffect))]
        private void AddNewResourceIncreaserEffect()
        {
            _effects.Add(new ResourceIncreaserEffect());
        }
    }
}