using UnityEngine;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private IntReference _playerKills;
    [SerializeField] private bool _resetOnAwake;

    private void Awake()
    {
        _playerKills.Value = 0;
    }

    public void IncreaseKillCounter()
    {
        _playerKills.Value++;
    }
}
