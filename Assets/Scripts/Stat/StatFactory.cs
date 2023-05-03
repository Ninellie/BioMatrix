using UnityEngine;
public class StatFactory : MonoBehaviour
{
    private GameTimeScheduler _gameTimeScheduler;
    private void Awake()
    {
        _gameTimeScheduler = GetComponent<GameTimeScheduler>();
    }

    public Stat GetStat(float baseValue)
    {
        return new Stat(baseValue, _gameTimeScheduler);
    }
}
