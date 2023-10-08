using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Repulse
{
    public class CageRepulse : MonoBehaviour
    {
        [SerializeField] private float _repulseForce;
        [SerializeField] private string _layerName;

        private void OnCollisionStay2D(Collision2D collision)
        {
            var otherCollider2D = collision.collider;

            if (otherCollider2D.gameObject.layer != LayerMask.NameToLayer(_layerName)) return;
        }
    }
}