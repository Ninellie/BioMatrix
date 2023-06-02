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
    
    public static void SetPropValueByPath(object target, string path, object value)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        var names = path.Split('.');

        var i = names.Length;

        string propName = null;

        foreach (var name in names)
        {
            i--;
            if (i == 0)
            {
                propName = name;
                break;
            }
            target = target.GetType().GetProperty(name)?.GetValue(target, null);
            if (target == null)
            {
                throw new ArgumentException("Invalid path", nameof(path));
            }
        }

        PropertyInfo prop = target.GetType().GetProperty(propName);
        prop.SetValue(target, value);
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