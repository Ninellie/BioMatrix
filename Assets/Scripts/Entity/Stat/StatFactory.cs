using UnityEngine;
public class StatFactory : MonoBehaviour
{
    //[SerializeField] private GameTimeScheduler _gameTimeScheduler;
    public Stat GetStat(float baseValue)
    {
        return new Stat(baseValue);
    }
}