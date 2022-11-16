//using System;
//using UnityEngine;
//[CreateAssetMenu(fileName = "Operation", menuName = "New operation", order = 52)]
//public class NumericalModificationOperationAsset : ScriptableObject
//{
//    [SerializeField] private OperationType _type;
//    public OperationType Type => _type;
//    public float Operation(float baseValue, float modificationValue)
//    {
//        return _type switch
//        {
//            OperationType.Addition => baseValue + modificationValue,
//            OperationType.Multiplication => baseValue * modificationValue,
//            _ => throw new ArgumentOutOfRangeException()
//        };
//    }
//}