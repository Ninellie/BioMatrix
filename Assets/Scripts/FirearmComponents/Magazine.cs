//using UnityEngine;
//using System;

//public class Magazine : MonoBehaviour
//{
//    public Action onCurrentAmountChanged;
//    public float CurrentAmount
//    {
//        get => _currentAmount;
//        private set
//        {
//            Debug.Log($"Try to set current magazine amount of {gameObject.name} to value: {value}");
//            switch (value)
//            {
//                case < 0:
//                    _currentAmount = MinMagazineAmount;
//                    Debug.Log("Magazine of " + gameObject.name + " is empty");
//                    onEmpty?.Invoke();
//                    break;
//                case 0:
//                    _currentAmount = value;
//                    onEmpty?.Invoke();
//                    Debug.Log("Magazine of " + gameObject.name + " is empty");
//                    break;
//                case > 0:
//                    _currentAmount = value;
//                    break;
//            }
//            onCurrentAmountChanged?.Invoke();
//        }
//    }
//    private float _currentAmount;
//    private const int MinMagazineAmount = 0;
    
//    public bool IsEmpty => CurrentAmount == 0;
//    public Action onEmpty;
//    private int Size => (int)GetComponent<Firearm>().MagazineCapacity.Value;
//    private void Start()
//    {
//        FullFill();
//    }
//    public void FullFill()
//    {
//        CurrentAmount = Size;
//    }
//    public void Pop()
//    {
//        CurrentAmount--;
//    }
//}
