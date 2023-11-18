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
        [field: SerializeField] public bool IsInProcess { get; private set; }
        [Space]
        [SerializeField] private GameEvent onReloadStart;
        [SerializeField] private GameEvent onReloadComplete;
        [Space]
        [SerializeField] private FloatReference reloadSpeedStat;
        [SerializeField] private FloatReference maxMagazineAmountStat;
        [SerializeField] private IntVariable magazine;

        private void Initiate()
        {
            IsInProcess = true;
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
            IsInProcess = false;
            magazine.SetValue((int)maxMagazineAmountStat.Value);
            onReloadComplete.Raise();
        }
    }
}
