using System.Collections;
using Assets.Scripts.Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.FirearmComponents
{
    public enum CastMode
    {
        BeforeDelay,
        AfterDelay
    }

    /// <summary>
    /// Caster Component invokes a cast event when enabled at given cast speed
    /// </summary>
    public class Caster : MonoBehaviour
    {
        [SerializeField] private FloatReference _castsPerSecond;
        [SerializeField] private CastMode _mode;
        [SerializeField] private UnityEvent _onCast;

        private void OnEnable()
        {
            StartCoroutine(Co_Cast());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator Co_Cast()
        {
            while (true)
            {
                switch (_mode)
                {
                    case CastMode.BeforeDelay:
                        _onCast.Invoke();
                        yield return new WaitForSeconds(1f / _castsPerSecond);
                        break;
                    case CastMode.AfterDelay:
                        yield return new WaitForSeconds(1f / _castsPerSecond);
                        _onCast.Invoke();
                        break;
                }
            }
        }
    }
}