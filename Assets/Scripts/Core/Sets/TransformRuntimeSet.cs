using UnityEngine;

namespace Assets.Scripts.Core.Sets
{
    [CreateAssetMenu(fileName = "New Transform Set", menuName = "Sets/Transform Thing", order = 51)]
    public class TransformRuntimeSet : RuntimeSet<TransformThing>
    {
    }
}