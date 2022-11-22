using UnityEngine;
using System;

public class Magazine : MonoBehaviour
{
    [SerializeField] private float _currentAmount;
    //private int size => GetComponent<FirearmSettings>().MagazineSize;
    public float size;
    public float CurrentAmount
    {
        get => _currentAmount;
        private set => _currentAmount = value;
    }
    public bool IsEmpty => CurrentAmount == 0;
    public static Action onEmpty;
    //void Start()
    //{
    //    FullFill();
    //}
    public void FullFill()
    {
        CurrentAmount = size;
    }
    public void Pop()
    {
        CurrentAmount--;
    }
}
