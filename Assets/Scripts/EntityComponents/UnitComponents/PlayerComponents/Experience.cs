using Assets.Scripts.Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private StatReference _experienceMultiplier;
        [SerializeField] private StatReference _experiencePerShard;
        [SerializeField] private Reserve _experienceReserve;

        private void Awake()
        {
            if (_experienceReserve == null) _experienceReserve = GetComponent<Reserve>();
        }

        public void IncreaseExperience()
        {
            var amount = _experiencePerShard * _experienceMultiplier;
            _experienceReserve.Increase(amount);
        }
    }
}