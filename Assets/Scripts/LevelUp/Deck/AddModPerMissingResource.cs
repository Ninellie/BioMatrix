using System;

public class AddModPerMissingResource : IEffect
{
    /* Следит сразу за двумя событиями: изменение текущего значения и изменение максимального значение,
     * После чего обновляет модификатор к стату, умножая его на разница между максимальным и текущим
     *
     */
    public string Name { get; }
    public StatModifier Modifier { get; }
    public string ModifiedStatName { get; } // Например Damage
    public string TargetName { get; set; } 
    //public string TriggerCurrentName { get; } // Например onCurrentLifePointsChanged
    public string TriggerMaxStatName { get; } // Например MaxLifePoints
    public string TriggerCurrentResourceName { get; } // Например MaxLifePoints

    private Action _triggerCurrentAction;
    private Action _triggerMaxAction;
    private Stat _stat;
    private Resource _currentResource;

    public void Attach(Entity target)
    {
        _stat = target.GetStatByName(ModifiedStatName);
        //_triggerCurrentAction = target.GetResourceByName(TriggerCurrentResourceName).onValueChanged;
        //_triggerMaxAction = target.GetStatByName(TriggerMaxStatName).onValueChanged;
    }

    public void Detach(Entity target)
    {

    }
    public void Subscribe(Entity target)
    {
        _triggerCurrentAction += UpdateMod;
        _triggerMaxAction += UpdateMod;
    }
    public void Unsubscribe(Entity target)
    {
        _triggerCurrentAction -= UpdateMod;
        _triggerMaxAction -= UpdateMod;
    }
    private void UpdateMod()
    {
        //var difValue = _maxValueStat.Value - _currentValue;
        //var newValue = Modifiers.Value * difValue;
        //StatModifier mod = new StatModifier(Modifiers.Type, difValue, Modifiers.IsTemporary, Modifiers.Duration);
        //_stat.AddModifier(mod);
    }
}