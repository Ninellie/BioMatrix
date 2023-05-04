using UnityEngine;
public class StatFactory : MonoBehaviour
{
    [SerializeField] private GameTimeScheduler _gameTimeScheduler;
    private void Awake()
    {
        //_gameTimeScheduler = GetComponent<GameTimeScheduler>();
    }

    public Stat GetStat(float baseValue)
    {
        return new Stat(_gameTimeScheduler, baseValue);
    }
}