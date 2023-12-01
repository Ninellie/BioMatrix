using UnityEngine;

namespace Assets.Scripts.Core.Variables.Components
{
    public class GameObjectSetter : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable _variable;

        private void Awake()
        {
            _variable.value = gameObject;
        }
    }
}