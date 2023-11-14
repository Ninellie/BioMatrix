using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    public class HealthReserve : MonoBehaviour
    {
        [SerializeField] private IntReference _currentHealth;

        [SerializeField] private StatReference _maximumHealth;
        [SerializeField] private int _edgeValue;

        public GameObjectGameEvent onDeath;
        public GameEvent onEdge;
        public GameEvent onDecrease;

        public void TakeDamage(int damage)
        {
            var oldValue = _currentHealth.Value;
            var newValue = _currentHealth.Value -= damage;
            
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