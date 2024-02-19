using Assets.Scripts.FirearmComponents;
using UnityEngine;

namespace Assets.Scripts.Core.Sets
{
    [CreateAssetMenu(fileName = "New Shooter Set", menuName = "Sets/Shooter", order = 51)]
    public class ShooterRuntimeSet : RuntimeSet<Shooter>
    {
    }
}