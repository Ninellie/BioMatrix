using UnityEngine;

namespace Assets.Scripts.Core.Sets
{
    [CreateAssetMenu(fileName = "New Thing Set", menuName = "Sets/Thing", order = 51)]
    public class ThingRuntimeSet : RuntimeSet<Thing>
    { }
}