using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace FirearmComponents
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
        [field:SerializeField] public bool Suspended { get; set; }
        [SerializeField] private FloatReference _castsPerSecond;
        [SerializeField] private CastMode _mode;
        [SerializeField] private List<ConditionComponent> _conditions;
        [SerializeField] private bool _allConditionsAreMet;
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
                if (Suspended)
                {
                    yield return new WaitWhile(() => Suspended);
                }
                switch (_mode)
                {
                    case CastMode.BeforeDelay:
                        CheckConditions();
                        if (_allConditionsAreMet)
                        {
                            _onCast.Invoke();
                        }
                        yield return new WaitForSeconds(1f / _castsPerSecond);
                        break;
                    case CastMode.AfterDelay:
                        yield return new WaitForSeconds(1f / _castsPerSecond);
                        CheckConditions();
                        if (_allConditionsAreMet)
                        {
                            _onCast.Invoke();
                        }
                        break;
                }
            }
        }

        private void CheckConditions()
        {
            _allConditionsAreMet = _conditions.All(condition => condition.IsMet());
        }
    }
}