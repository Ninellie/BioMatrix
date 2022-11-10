using UnityEngine;
using System;

public class Magazine : MonoBehaviour
{
    public int CurrentAmount { get; set; }
    private int Size => GetComponent<FirearmSettings>().MagazineSize;
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
}
