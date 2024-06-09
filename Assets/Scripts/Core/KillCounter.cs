using Core.Variables;
using UnityEngine;

namespace Core
{
    public class KillCounter : MonoBehaviour
    {
        [SerializeField] private IntVariable _playerKills;
        [SerializeField] private bool _resetOnAwake;

        private void Awake()
        {
            if (!_resetOnAwake) return;
            _playerKills.SetValue(0);
        }

        public void IncreaseKillCounter()
        {
            _playerKills.ApplyChange(1);
        }
    }
}
