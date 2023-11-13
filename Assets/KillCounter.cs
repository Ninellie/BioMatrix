using UnityEngine;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private IntReference _playerKills;

    public void IncreaseKillCounter()
    {
        _playerKills.Value++;
    }
}
