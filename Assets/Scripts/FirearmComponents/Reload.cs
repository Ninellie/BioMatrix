using System.Collections;
using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Variables;
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

    public class Reload : MonoBehaviour
    {
        [SerializeField] private GameObject _plateUi;
        [SerializeField] private bool _isInProgress;

        public GameEvent onReloadStart;
        public GameEvent onReloadComplete;

        public FloatReference reloadSpeedStat;
        public FloatReference maxMagazineAmountStat;
        public IntVariable magazine;
        public bool IsInProcess => _isInProgress;

        private bool _isSubscribed;

        private void Initiate()
        {
            _isInProgress = true;
            onReloadStart.Raise();

            var reloadTime = 1 / reloadSpeedStat.Value;
            var isInstant = !(reloadTime > 0);

            switch (isInstant)
            {
                case false:
                    Invoke(nameof(Complete), reloadTime);
                    break;
                case true:
                    Complete();
                    break;
            }
        }

        private void Complete()
        {
            _isInProgress = false;
            magazine.SetValue((int)maxMagazineAmountStat.Value);
            onReloadComplete.Raise();
        }
    }
}
