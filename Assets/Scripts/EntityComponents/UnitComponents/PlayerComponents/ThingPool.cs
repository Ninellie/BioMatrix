using System.Linq;
using Assets.Scripts.Core.Sets;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class ThingPool : MonoBehaviour
    {
        public ThingRuntimeSet disabledSet;
        public ThingRuntimeSet enabledSet;
        public bool allowGettingActiveObjects;

        public Thing Get()
        {
            if (disabledSet.items.Count == 0)
            {
                if (!allowGettingActiveObjects) return null;
                if (enabledSet.items.Count == 0)
                {
                    return null;
                }
                return enabledSet.items.First();
            }
            var firstDisabled = disabledSet.items.First();
            firstDisabled.gameObject.SetActive(true);
            return firstDisabled;
        }

        public void TryEnable()
        {
            if (disabledSet != null) disabledSet.items.FirstOrDefault()?.gameObject.SetActive(true);
        }

        public void TryDisable()
        {
            if (enabledSet != null) enabledSet.items.FirstOrDefault()?.gameObject.SetActive(false);
        }
    }
}