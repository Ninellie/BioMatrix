using Assets.Scripts.Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public enum DelayMode
    {
        Reset, // Каждый Delay сбрасывает время до _time
        Additive, // Каждый Delay добавляет _time времени до вызова Cast
        Singular, // Пока Cast отложен, Delay ничего не делает
        Separate // Каждый Delay ведёт вызывает Cast через время _time
    }
    public class CastDelayer : MonoBehaviour // TODO Переместить файл скрипта в более общую директорию
    {
        [SerializeField] private FloatReference _time;
        [SerializeField] private DelayMode _mode;
        [SerializeField] private UnityEvent _onCast;

        private float _lastDelayTimeSinceLevelLoad;
        private float _delayTime;

        /// <summary>
        /// Вызывает событие _onCast спустя время _time по принципу, определённому полем _mode
        /// </summary>
        public void Delay()
        {
            switch (_mode)
            {
                case DelayMode.Reset:
                    CancelInvoke(nameof(Cast));
                    Invoke(nameof(Cast), _time);
                    break;
                case DelayMode.Additive:
                    if (!IsInvoking(nameof(Cast)))
                    {
                        _lastDelayTimeSinceLevelLoad = Time.timeSinceLevelLoad;
                        _delayTime = _time;
                        Invoke(nameof(Cast), _delayTime);
                        break;
                    }
                    var timeDifference = Time.timeSinceLevelLoad - _lastDelayTimeSinceLevelLoad;
                    _lastDelayTimeSinceLevelLoad = Time.timeSinceLevelLoad;
                    _delayTime -= timeDifference;
                    _delayTime += _time;
                    CancelInvoke(nameof(Cast));
                    Invoke(nameof(Cast), _delayTime);
                    break;
                case DelayMode.Singular:
                    if (IsInvoking(nameof(Cast))) break;
                    CancelInvoke(nameof(Cast)); 
                    Invoke(nameof(Cast), _time);
                    break;
                case DelayMode.Separate:
                    Invoke(nameof(Cast), _time);
                    break;
            }
        }

        private void Cast()
        {
            _onCast.Invoke();
            _delayTime = 0;
        }
    }
}