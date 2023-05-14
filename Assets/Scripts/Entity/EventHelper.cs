using System;

public static class EventHelper
{
    public static void AddActionByName(object target, string actionName, Action method)
    {
        if (actionName is nameof(IEffect.Attach) or nameof(IEffect.Detach)) return;
        var eventInfo = target.GetType().GetEvent(actionName);
        eventInfo.AddEventHandler(target, method);
    }
    public static void RemoveActionByName(object target, string actionName, Action method)
    {
        var eventInfo = target.GetType().GetEvent(actionName);
        eventInfo.RemoveEventHandler(target, method);
    }
    public static object GetPropByName(object target, string propName)
    {
        if (string.IsNullOrEmpty(propName))
        {
            return target;
        }

        var names = propName.Split('.');

        foreach (var name in names)
        {
            target = target.GetType().GetProperty(name).GetValue(target, null);
        }

        return target;
    }
}