using UnityEngine;
using System;

public class Magazine : MonoBehaviour
{
    public float CurrentAmount { get; private set; }
    public bool IsEmpty => CurrentAmount == 0;
    public static Action onEmpty;
    private int Size => (int)GetComponent<Firearm>().MagazineSize.Value;
    private void Start()
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
        if (CurrentAmount != 0) return;
        Magazine.onEmpty?.Invoke();
        UnityEngine.Debug.Log("Magazine of " + gameObject.name + " is empty");
    }
}
