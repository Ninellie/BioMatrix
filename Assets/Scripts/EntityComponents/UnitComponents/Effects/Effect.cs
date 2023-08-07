using System;
using UnityEngine;

[Serializable]
public class Effect : IEffect
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;
    public string Name => _name;
    public string Description => _description;

    public virtual void Activate()
    {
    }

    public virtual void Deactivate()
    {
    }
}