using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Sets
{
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
}