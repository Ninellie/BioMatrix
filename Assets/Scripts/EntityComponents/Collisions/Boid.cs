using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Variables.References;
using EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace EntityComponents.Collisions
{
    public class Boid : MonoBehaviour
    {
        public MovementController controller;

        public List<Transform> neighbours;
        public bool applyCohesion;
        public bool normalizeCohesion;
        public FloatReference cohesionPower;
        public bool applySeparation;
        public bool normalizeSeparation;
        public FloatReference separationPower;

        public float checkPeriod;


        public Vector2 velocity;
        public Transform myTransform;
        private void Awake()
        {
            velocity = Vector2.zero;
            neighbours = new List<Transform>();
            if (myTransform != null) return;
            myTransform = transform;
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.isTrigger) return;
            if (collider2D.tag != tag) return;
            if (neighbours.Contains(collider2D.transform)) return;
            neighbours.Add(collider2D.gameObject.transform);
        }

        private void OnTriggerExit2D(Collider2D collider2D)
        {
            if (collider2D.isTrigger) return;
            if (collider2D.tag != tag) return;
            if (!neighbours.Contains(collider2D.transform)) return;
            neighbours.Remove(collider2D.transform);
        }

        private void OnEnable()
        {
            StartCoroutine(UpdateVelocityCheck());
        }

        private void OnDisable()
        {
            StopCoroutine(UpdateVelocityCheck());
        }

        private IEnumerator UpdateVelocityCheck()
        {
            while (true)
            {
                controller.AddVelocity(velocity * -1);
                if (neighbours.Count == 0)
                {
                    velocity = Vector2.zero;
                }
                else
                {
                    UpdateVelocity();
                }
                controller.AddVelocity(velocity);
                Debug.DrawRay(myTransform.position, velocity, Color.blue);
                yield return new WaitForSeconds(checkPeriod);
            }
        }

        private void UpdateVelocity()
        {
            if (applyCohesion)
            {
                velocity += GetCohesionVelocity();
            }
            if (applySeparation)
            {
                velocity += GetSeparationVelocity();
            }
        }

        private Vector2 GetCohesionVelocity()
        {
            var center = neighbours.Aggregate(Vector2.zero, (current, neighbour) => current + (Vector2)neighbour.position);
            center /= neighbours.Count;
            var cohesionVelocity = center - (Vector2)myTransform.position;
            cohesionVelocity *= cohesionPower;
            if (normalizeCohesion)
            {
                cohesionVelocity.Normalize();
            }
            return cohesionVelocity;
        }

        private Vector2 GetSeparationVelocity()
        {
            var separationVelocity = Vector2.zero;
            foreach (var neighbour in neighbours)
            {
                var distance = Vector2.Distance(myTransform.position, neighbour.position);
                separationVelocity += (Vector2)(myTransform.position - neighbour.position) / distance;
            }
            separationVelocity /= neighbours.Count;
            separationVelocity *= separationPower;
            if (normalizeSeparation)
            {
                separationVelocity.Normalize();
            }
            return separationVelocity;
        }
    }
}