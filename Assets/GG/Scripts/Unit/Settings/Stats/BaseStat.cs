//using System.Collections.Generic;
//using UnityEngine;



//public class BaseStat : ScriptableObject
//{
//    [SerializeField] private string _name;
//    [SerializeField] private string _description;
//    [SerializeField] private float _baseValue;
//    [SerializeField] private bool _isModifiable;
//    [SerializeField] private List<NumericalModificationOperationAsset> _validOperationsList;
//    public string Name => _name;
//    public string Description => _description;
//    public float BaseValue => _baseValue;
//    public bool IsModifiable => _isModifiable;
//    public List<NumericalModificationOperationAsset> ValidOperationsList => _validOperationsList;
//    public bool IsValidModifier(NumericalStatModifier modifier)
//    {
//        return _validOperationsList.Contains(modifier.Operation);
//    }
//}