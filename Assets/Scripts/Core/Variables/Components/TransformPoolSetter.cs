using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.Core.Variables.Components
{
    public class TransformPoolSetter : MonoBehaviour
    {
        [SerializeField] private TransformPoolVariable _variable;

        private void Awake()
        {
            _variable.value = GetComponent<TransformPool>();
        }
    }
}