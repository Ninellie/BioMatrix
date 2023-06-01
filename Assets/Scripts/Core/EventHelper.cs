using System;
using System.Reflection;

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
    
    public static object SetPropValueByPath(object target, string path, object value)
    {
        if (string.IsNullOrEmpty(path))
        {
            return target;
        }

        var names = path.Split('.');

        var i = names.Length;

        foreach (var name in names)
        {
            i--;
            if (i == 0)
            {
                PropertyInfo prop = target.GetType().GetProperty(name);
                prop.SetValue(target, value);
            }
            target = target.GetType().GetProperty(name)?.GetValue(target, null);
        }

        return target;
    }
    
    public static object GetPropByPath(object target, string path)
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