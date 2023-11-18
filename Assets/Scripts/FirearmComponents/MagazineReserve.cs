using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    // Герой жмёт на кнопку стрельбы
    // Отсылается ивент стрельбы
    // Оружие начинает реагировать на ивент с DoAction
    // Выстрел
    // Минус 1 патрон
    // Подождать дилей скорости атаки
    // Если в магазине не осталось патронов, то запустить перезарядку

    public class MagazineReserve : MonoBehaviour
    {
        [field: SerializeField] public bool OnReload { get; private set; }
        [Space]
        [SerializeField] private IntReference _value;
        [Space]
        [SerializeField] private GameEvent _onReloadStart;
        [SerializeField] private GameEvent _onReloadComplete;
        [Space]
        [SerializeField] private FloatReference _reloadSpeedStat;
        [SerializeField] private FloatReference _maxMagazineCapacity;

        public int Value => _value.Value;

        public void Pop()
        {
            if (OnReload) return;
            if (_value.useConstant)
            {
                _value.constantValue--;
            }
            else
            {
                _value.variable.ApplyChange(-1);
            }

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

        private void Fill()
        {
            if (_value.useConstant)
                _value.constantValue = (int)_maxMagazineCapacity.Value;
            else
                _value.variable.SetValue((int)_maxMagazineCapacity.Value);
        }
    }
}