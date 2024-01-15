using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Collisions
{
    public class Boid : MonoBehaviour
    {
        public List<Transform> neighbours;
        [Range(-10,  10)] public float cohesionPower;
        [Range(-10,  10)] public float separationPower;
        public Vector2 velocity;
        public Transform movingTransform;

        private void Awake()
        {
            movingTransform = ((Component)this).transform;
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.tag != tag) return;
            if (neighbours.Contains(collider2D.transform)) return;
            neighbours.Add(collider2D.gameObject.transform);
        }

        private void OnTriggerExit2D(Collider2D collider2D)
        {
            if (collider2D.tag != tag) return;
            if (!neighbours.Contains(collider2D.transform)) return;
            neighbours.Remove(collider2D.transform);
        }

        private void FixedUpdate()
        {
            UpdateVelocity();
            Debug.DrawRay(movingTransform.position, velocity, Color.magenta);
            movingTransform.Translate(velocity);
        }

        private void UpdateVelocity()
        {
            var cohesion = GetCohesionVelocity();
            var separation = GetSeparationVelocity();
            velocity = cohesion + separation;
        }

        private Vector2 GetCohesionVelocity()
        {
            var center = neighbours.Aggregate(Vector2.zero, (current, neighbour) => current + (Vector2)neighbour.position);
            center /= neighbours.Count;
            var cohesionVelocity = center - (Vector2)movingTransform.position;
            cohesionVelocity *= cohesionPower;
            return cohesionVelocity;
        }

        private Vector2 GetSeparationVelocity()
        {
            var separationVelocity = Vector2.zero;
            foreach (var neighbour in neighbours)
            {
                var distance = Vector2.Distance(movingTransform.position, neighbour.position);
                separationVelocity += (Vector2)(movingTransform.position - neighbour.position) / distance;
            }
            separationVelocity /= neighbours.Count;
            separationVelocity *= separationPower;
            return separationVelocity;
        }
    }
}