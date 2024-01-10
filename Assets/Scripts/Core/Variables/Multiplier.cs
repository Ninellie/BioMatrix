using Assets.Scripts.Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Scripts.Core.Variables
{
    public class Multiplier : MonoBehaviour
    {
        [SerializeField] private FloatReference _multiplier;
        [SerializeField] private FloatReference _baseValue;
        [SerializeField] private UnityEvent<int> _result;

        public void Multiply()
        {
            var amount = _baseValue.Value * _multiplier.Value;
            _result.Invoke((int)amount);
        }
    }
}