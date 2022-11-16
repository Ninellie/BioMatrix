//using UnityEngine;

//public class Stats : MonoBehaviour
//{
//    [SerializeField] private NumericalStat[] _actualStats;
//    [SerializeField] private BaseStatSheet _baseStatSheet;
//    private void Awake()
//    {
//        _actualStats = new NumericalStat[_baseStatSheet.Stats.Length];
//        Debug.Log(_baseStatSheet.Stats.Length.ToString());
//        SetStats();
//    }
//    private void SetStats()
//    {
//        for (int i = 0; i < _baseStatSheet.Stats.Length; i++)
//        {
//            _actualStats[i] = new NumericalStat(_baseStatSheet.Stats[i]);
//            Debug.Log(_baseStatSheet.Stats[i].Name); 
//        }
//    }
//}