using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public interface ICondition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Is this condition met</returns>
        bool IsMet();
    }

    /// <summary>
    /// Caster Component invokes a cast event when enabled at given cast speed
    /// </summary>
    public class Caster : MonoBehaviour
    {
        [SerializeField] private FloatReference _castsPerSecond;
        [SerializeField] private CastMode _mode;
        [SerializeField] private UnityEvent _onCast;
        // TODO Добавить условия на выбор: например, каст только тогда, когда дистанция между объектом меньше. Условия могут быть скриптаблами

        [SerializeField] private List<ConditionComponent> _conditions;

        [SerializeField] private bool _allConditionsAreMet;

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
                        _allConditionsAreMet = _conditions.All(condition => condition.IsMet());
                        if (_allConditionsAreMet)
                        {
                            _onCast.Invoke();
                        }
                        yield return new WaitForSeconds(1f / _castsPerSecond);
                        break;
                    case CastMode.AfterDelay:
                        yield return new WaitForSeconds(1f / _castsPerSecond);
                        _allConditionsAreMet = _conditions.All(condition => condition.IsMet());
                        if (_allConditionsAreMet)
                        {
                            _onCast.Invoke();
                        }
                        break;
                }
            }
        }
    }
}