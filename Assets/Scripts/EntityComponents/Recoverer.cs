using System.Collections;
using Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.Resources
{
    /// <summary>
    /// Посылает ивент каждый определённый промежуток времени. Промежуток времени определяется переменными speed и rate.
    /// Отсчёт до восстановления начинается как только currentValue станет меньше чем limiter.
    /// Speed - значение восстановления в минуту. Это то, за сколько минут восстановится 1 единица value.
    /// </summary>
    public class Recoverer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private IntReference _currentValue;
        [SerializeField] private FloatReference _limiter;
        [SerializeField] [Tooltip("Recovering speed value per minute")]private FloatReference _speed;
        [SerializeField] [Min(0)] private float _rate = 1;
        [Header("Response")]
        [SerializeField] private UnityEvent _onRecoverUnityEvent;
        [Header("Indicator")]
        [SerializeField] private float _recoverValue;

        private void OnEnable()
        {
            StartCoroutine(Recover());
        }

        private void OnDisable()
        {
            StopCoroutine(Recover());
        }

        private IEnumerator Recover()
        {
            while (true)
            {
                yield return new WaitWhile(() => !(_currentValue.Value < _limiter.Value));
                _recoverValue += _speed.Value / 60 * _rate;
                yield return new WaitForSeconds(_rate);
                if (!(_recoverValue > 1)) continue;
                _recoverValue = 0f;
                _onRecoverUnityEvent.Invoke();
            }
        }
    }
}