using Assets.Scripts.Core.Variables;
using UnityEngine;

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
