using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Collisions
{
    public class Boid : MonoBehaviour
    {
        public List<GameObject> neighbours;

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.tag != tag) return;
            _onTriggerEnter2D.Invoke(collider2D);
        }
    }
}