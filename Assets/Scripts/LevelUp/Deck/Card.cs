using System;
using UnityEngine;

public interface IEffect
{
    string Name { get; }
    void Attach(Player player);
    void Subscribe();
    void Unsubscribe();
}

public class PerMissingMod : IEffect
{
    /* Следит сразу за двумя событиями: изменение текущего значения и изменение максимального значение,
     * После чего обновляет модификатор к стату, умножая его на разница между максимальным и текущим
     *
     */

    public string Name { get; }

    public StatModifier Modifier { get; }
    public string StatName { get; } // Например Damage
    public string TriggerCurrentName { get; } // Например onCurrentLifePointsChanged
    public string TriggerMaxStatName { get; } // Например MaxLifePoints
    public string MaxName { get; }


    private Action _triggerCurrentAction;
    private Action _triggerMaxAction;
    private Stat _stat;
    private float _currentValue;
    private Stat _maxValueStat;

    public void Attach(Player player)
    {
        _stat = player.GetStatByName(StatName);
        _triggerCurrentAction = player.GetActionByName(TriggerCurrentName);
        _maxValueStat = player.GetStatByName(TriggerMaxStatName);
        _triggerMaxAction = _maxValueStat.onValueChanged;

    }
    public void Subscribe()
    {
        _triggerCurrentAction += UpdateMod;
        _triggerMaxAction += UpdateMod;
    }
    public void Unsubscribe()
    {
        _triggerCurrentAction -= UpdateMod;
        _triggerMaxAction -= UpdateMod;
    }
    private void UpdateMod()
    {
        var difValue = _maxValueStat.Value - _currentValue;
        var newValue = Modifier.Value * difValue;
        StatModifier mod = new StatModifier(Modifier.Type, difValue, Modifier.IsTemporary, Modifier.Duration);


        _stat.AddModifier(mod);
    }
}

public class AddModOn : IEffect
{
    public string Name { get; }
    public StatModifier Modifier { get; }
    public string StatName { get; }
    public string TriggerName { get; } // Имя переменной события типа Action, на которое будет полписан AddMod()

    private Action _triggerAction; // Ссылка на событие, на которое сработает AddMod
    private Stat _stat;

    public void Attach(Player player) // поменять player на таргет
    {
        _stat = player.GetStatByName(StatName);
        _triggerAction = player.GetActionByName(TriggerName);
    }
    public void Subscribe()
    {
        _triggerAction += AddMod;
    }
    public void Unsubscribe()
    {
        _triggerAction -= AddMod;
    }
    private void AddMod()
    {
        _stat.AddModifier(Modifier);
    }
}


[Serializable]
public class Card
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int DropWeight { get; set; }
    public string[] InfluencedStats { get; set; }
    public StatModifier[] ModifierList { get; set; }
    public IEffect[] Effects { get; set; }
}