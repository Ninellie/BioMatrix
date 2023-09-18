using System;
using UnityEngine;

[Serializable]
public class Effect : IEffect
{
    [SerializeField] protected string _name;
    [SerializeField, Multiline] protected string _description;
    [SerializeField] protected TargetName _targetName;
    public string Name => _name;

    public string Description => _description;
    public TargetName TargetName => _targetName;

    public virtual void Activate()
    {
    }

    public virtual void Deactivate()
    {
    }
}