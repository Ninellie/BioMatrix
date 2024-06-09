using Core.Events;
using Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace FirearmComponents
{
    public class MagazineReserve : MonoBehaviour
    {
        [field: SerializeField] public bool OnReload { get; private set; }
        [Space]
        [SerializeField] private IntReference _value;
        [SerializeField] private bool _fillOnStart;
        [Space]
        [SerializeField] private GameEvent _onReloadStart;
        [SerializeField] private GameEvent _onReloadComplete;
        [SerializeField] private UnityEvent _onChange;
        [Space]
        [SerializeField] private FloatReference _reloadSpeedStat;
        [SerializeField] private FloatReference _maxMagazineCapacity;
        [SerializeField] private IntReference _ammoPerShoot;

        public int Value => _value.Value;

        private void Start()
        {
            if (!_fillOnStart) return;
            Fill();
        }

        public void Pop()
        {
            if (_value == 0)
            {
                InitiateReload();
                return;
            }
            if (OnReload) return;
            ApplyChange(_ammoPerShoot * -1);
            _onChange.Invoke();
            if (_value == 0)
            {
                InitiateReload();
            }
        }

        private void InitiateReload()
        {
            OnReload = true;
            _onReloadStart.Raise();

            var reloadTime = 1 / _reloadSpeedStat.Value;
            var isInstant = !(reloadTime > 0);

            switch (isInstant)
            {
                case false:
                    Invoke(nameof(CompleteReload), reloadTime);
                    break;
                case true:
                    CompleteReload();
                    break;
            }
        }

        private void CompleteReload()
        {
            OnReload = false;
            Fill();
            _onReloadComplete.Raise();
        }

        private void ApplyChange(int amount)
        {
            if (_value.useConstant)
                _value.constantValue += amount;
            else
                _value.variable.ApplyChange(amount);
        }

        private void Fill()
        {
            if (_value.useConstant)
                _value.constantValue = (int)_maxMagazineCapacity.Value;
            else
                _value.variable.SetValue((int)_maxMagazineCapacity.Value);

            _onChange.Invoke();
        }
    }
}