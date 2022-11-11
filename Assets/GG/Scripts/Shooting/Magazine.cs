using UnityEngine;
using System;

public class Magazine : MonoBehaviour
{
    [SerializeField] private int _currentAmount;
    private int Size => GetComponent<FirearmSettings>().MagazineSize;
    public int CurrentAmount
    {
        get => _currentAmount;
        private set => _currentAmount = value;
    }
    public bool IsEmpty => CurrentAmount == 0;
    public static Action OnEmpty;
    void Start()
    {
        FullFill();
    }
    public void FullFill()
    {
        CurrentAmount = Size;
    }
    public void Pop()
    {
        CurrentAmount--;
    }
}
