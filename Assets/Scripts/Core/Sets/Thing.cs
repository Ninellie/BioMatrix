using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.Sets
{
    //[CreateAssetMenu(order = 51)]
//public class VisibleThingRuntimeSet : RuntimeSet<VisibleThing>
//{ }


//public class VisibleThing : MonoBehaviour
//{
//    public VisibleThingRuntimeSet runtimeSet;

//    private void OnBecameVisible()
//    {
//        runtimeSet.Add(this);
//    }

//    private void OnBecameInvisible()
//    {
//        runtimeSet.Remove(this);
//    }
//}

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
}