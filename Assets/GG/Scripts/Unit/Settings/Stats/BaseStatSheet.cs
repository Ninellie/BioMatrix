using UnityEngine;

[CreateAssetMenu(fileName = "BaseStat sheet", menuName = "New stat sheet", order = 53)]
public class BaseStatSheet : ScriptableObject
{
    [SerializeField] private BaseStat[] _stats;
    public BaseStat[] Stats => _stats;
}