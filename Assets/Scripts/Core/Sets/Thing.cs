using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(order = 51)]
public class ThingRuntimeSet : RuntimeSet<Thing>
{ }

[CreateAssetMenu(order = 51)]
public class VisibleThingRuntimeSet : RuntimeSet<VisibleThing>
{ }

public abstract class RuntimeSet<T> : ScriptableObject
{
    public List<T> items = new();

    public void Add(T thing)
    {
        if (!items.Contains(thing))
            items.Add(thing);
    }

    public void Remove(T thing)
    {
        if (items.Contains(thing))
            items.Remove(thing);
    }
}

public class VisibleThing : MonoBehaviour
{
    public VisibleThingRuntimeSet runtimeSet;

    private void OnBecameVisible()
    {
        runtimeSet.Add(this);
    }

    private void OnBecameInvisible()
    {
        runtimeSet.Remove(this);
    }
}

public class Thing : MonoBehaviour
{
    public ThingRuntimeSet runtimeSet;

    private void OnEnable()
    {
        runtimeSet.Add(this);
    }

    private void OnDisable()
    {
        runtimeSet.Remove(this);
    }
}

public class ThingDisabler : MonoBehaviour
{
    public ThingRuntimeSet set;

    public void DisableAll()
    {
        // Loop backwards since the list may change when disabling
        for (int i = set.items.Count - 1; i >= 0; i--)
        {
            set.items[i].gameObject.SetActive(false);
        }
    }

    public void DisableRandom()
    {
        var index = Random.Range(0, set.items.Count);
        set.items[index].gameObject.SetActive(false);
    }
}

public class ThingMonitor : MonoBehaviour
{
    public ThingRuntimeSet set;

    public Text Text;

    private int _previousCount = -1;

    private void OnEnable()
    {
        UpdateText();
    }

    private void Update()
    {
        if (_previousCount == set.items.Count) return;
        UpdateText();
        _previousCount = set.items.Count;
    }

    public void UpdateText()
    {
        Text.text = "There are " + set.items.Count + " things.";
    }
}