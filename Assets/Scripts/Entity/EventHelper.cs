using System;

public static class EventHelper
{
    public static void AddActionByName(object target, string actionName, Action method)
    {
        var eventInfo = target.GetType().GetEvent(actionName);
        eventInfo.AddEventHandler(target, method);
    }
    public static void RemoveActionByName(object target, string actionName, Action method)
    {
        var eventInfo = target.GetType().GetEvent(actionName);
        eventInfo.RemoveEventHandler(target, method);
    }
    public static object GetPropByName(object target, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return target;
        }

        var names = path.Split('.');

        foreach (var name in names)
        {
            target = target.GetType().GetProperty(name).GetValue(target, null);
        }

        return target;
    }
}