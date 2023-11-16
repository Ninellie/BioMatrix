using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    public class HealthReserve : MonoBehaviour
    {
        [SerializeField] private IntReference _currentHealth;
        [SerializeField] private StatReference _maximumHealth;
        [SerializeField] private int _edgeValue;
        public int Current => _currentHealth.Value;
        public bool Empty => _currentHealth.Value == 0;
        public bool Full => _currentHealth.Value == (int)_maximumHealth.Value;

        [SerializeField] private GameObjectGameEvent onDeath;
        [SerializeField] private GameEvent onEdge;
        [SerializeField] private GameEvent onDecrease;

        public void TakeDamage(int damage)
        {
            var oldValue = _currentHealth.Value;
            
            if (_currentHealth.useConstant)
            {
                _currentHealth.constantValue -= damage;
            }
            else
            {
                _currentHealth.variable.ApplyChange(-damage);
            }

            var newValue = _currentHealth.Value;
            
            if (onDecrease != null)
            {
                if (oldValue > newValue)
                {
                    onDecrease.Raise();
                }
            }

            if (onEdge != null)
            {
                if (newValue == _edgeValue)
                {
                    onEdge.Raise();
                }
            }

            if (onDeath == null) return;

            if (newValue == 0)
            { 
                onDeath.Raise(gameObject);
            }
        }
    }
}