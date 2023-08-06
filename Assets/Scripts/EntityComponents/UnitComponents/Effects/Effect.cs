using System;
using UnityEngine;

[Serializable]
public class Effect : IEffect
    //, IStackable
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;
    public string Name => _name;
    public string Description => _description;
    //[SerializeField] private int _maxStacks;
    
    //[SerializeField]
    //[ReadOnly]
    //private int _stacksCount;


    //public int StacksCount
    //{
    //    get => _stacksCount;
    //    set => _stacksCount = value;
    //}

    //public int MaxStacks => _maxStacks;

    public virtual void Activate()
    {
    }

    public virtual void Deactivate()
    {
    }

    public void AddStack()
    {
        throw new NotImplementedException();
    }

    public void RemoveStack()
    {
        throw new NotImplementedException();
    }
}