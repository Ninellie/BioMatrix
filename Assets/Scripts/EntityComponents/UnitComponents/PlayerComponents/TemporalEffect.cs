using UnityEngine;

namespace EntityComponents.UnitComponents.PlayerComponents
{
    public class TemporalEffect : MonoBehaviour
    {
        [SerializeField] private StackPool _pool;
        [SerializeField] [Range(0, 50)] private float _duration;

        public void TryAdd()
        {
            CancelInvoke(nameof(DisableAll));
            _pool.TryAdd();
            Invoke(nameof(DisableAll), _duration);
        }

        private void DisableAll()
        {
            _pool.DisableAll();
        }

        private void TryRelease()
        {
            _pool.TryRelease();
        }
    }
}