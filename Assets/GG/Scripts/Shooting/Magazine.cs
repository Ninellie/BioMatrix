using UnityEngine;
using System;

public class Magazine : MonoBehaviour
{
    //private int size => GetComponent<FirearmSettings>().MagazineSize;
    public float size;
    public float CurrentAmount { get; private set; }

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
        if (CurrentAmount != 0) return;
        Magazine.onEmpty?.Invoke();
        UnityEngine.Debug.Log("Magazine of " + gameObject.name + " is empty");
    }
}
