using System;

public class AddModOn : IEffect
{
    public string Name { get; }
    public StatModifier Modifier { get; }
    public string ModifiedStatName { get; } // Speed for example
    public string TriggerName { get; } // onValueChanged for example
    public string TriggerTypeName { get; } // Resource, Stat or Entity
    public string TriggerPropName { get; } // If TypeName == Resource or Stat

    private Stat _stat;

    public void Attach(Entity target)
    {
        _stat = target.GetStatByName(ModifiedStatName);
    }
    public void Subscribe(Entity target)
    {
        Action trigger;
        switch (TriggerTypeName)
        {
            case nameof(Resource):
                trigger = target.GetResourceByName(TriggerPropName).GetActionByName(TriggerName);
                trigger += AddMod;
                break;
            case nameof(Stat):
                trigger = target.GetStatByName(TriggerPropName).onValueChanged;
                trigger += AddMod;
                break;
            case nameof(Entity):
                trigger = target.GetActionByName(TriggerName);
                trigger += AddMod;
                break;
        }
    }
    public void Unsubscribe(Entity target)
    {
        Action trigger;
        switch (TriggerTypeName)
        {
            case nameof(Resource):
                trigger = target.GetResourceByName(TriggerPropName).GetActionByName(TriggerName);
                trigger -= AddMod;
                break;
            case nameof(Stat):
                trigger = target.GetStatByName(TriggerPropName).onValueChanged;
                trigger -= AddMod;
                break;
            case nameof(Entity):
                trigger = target.GetActionByName(TriggerName);
                trigger -= AddMod;
                break;
        }
    }
    private void AddMod()
    {
        _stat.AddModifier(Modifier);
    }
}