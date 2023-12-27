using Assets.Scripts.Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class CastDelayer : MonoBehaviour // TODO Переместить файл скрипта в более общую директорию
    {
        [SerializeField] private FloatReference _time;
        [SerializeField] private UnityEvent _onCast;

        public void Delay()
        {
            CancelInvoke(nameof(Cast));
            Invoke(nameof(Cast), _time);
        }

        private void Cast()
        {
            _onCast.Invoke();
        }
    }
}