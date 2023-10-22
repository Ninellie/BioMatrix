using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        [SerializeField] private Shield _shield;

        private KnockbackController _knockbackController;
        private Invulnerability _invulnerability;
        private ResourceList _resources;
        private PlayerMovementController _movementController;
        private Vector2 _cageCenter;

        private void Awake()
        {
            _resources = GetComponent<ResourceList>();
            _invulnerability = GetComponent<Invulnerability>();
            _knockbackController = GetComponent<KnockbackController>();
            _movementController = GetComponent<PlayerMovementController>();
        }

        private void OnCollisionStay2D(Collision2D collision2D)
        {
            switch (collision2D.collider.tag)
            {
                case "Cage":
                    Debug.LogWarning("CAGE_STAY");
                    PushBackInsideCage();
                    break;
            }
        }

        private void PushBackInsideCage()
        {
            var directionToCameraCenter = (_cageCenter - (Vector2)gameObject.transform.position).normalized;
            gameObject.transform.Translate(directionToCameraCenter * 10f);
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            var otherTag = collision2D.collider.tag;

            switch (otherTag)
            {
                case "Enemy":
                    var enemy = collision2D.gameObject.GetComponent<Enemy>();
                    CollideWithEnemy(enemy);
                    break;
                case "Enclosure":
                    Debug.LogWarning("ENCLOSURE");
                    var movementDirection = _movementController.GetRawMovementDirection() * -1;
                    CollideWithEnclosure(movementDirection);
                    break;
                case "Cage":
                    Debug.LogWarning("CAGE");
                    var directionToCameraCenter = (_cageCenter - (Vector2)gameObject.transform.position).normalized;
                    CollideWithEnclosure(directionToCameraCenter);
                    break;
            }
        }

        public void SetCageCenter(Vector2 center)
        {
            _cageCenter = center;
        }

        private void CollideWithEnclosure(Vector2 direction)
        {
            var force = direction * 50f;
            _knockbackController.Knockback(force);
            if (_invulnerability.IsInvulnerable) return;
            _invulnerability.ApplyInvulnerable();
            if (!_shield.Layers.IsEmpty)
            {
                _shield.Layers.Decrease();
            }
            else
            {
                //if (_resources.GetResource(ResourceName.Health).IsEmpty) return;
                //TakeDamage(1);
            }
        }

        private void CollideWithEnemy(Enemy enemy)
        {
            if (!_shield.Layers.IsEmpty)
                _shield.Layers.Decrease();
            else
            {
                var damageAmount = enemy.GetDamage();
                TakeDamage(damageAmount);
                if (_resources.GetResource(ResourceName.Health).IsEmpty) return;
                _invulnerability.ApplyInvulnerable();
                var knockbackPower = enemy.GetKnockbackPower();
                Vector2 enemyPosition = enemy.transform.position;
                KnockBackFromPosition(knockbackPower, enemyPosition);
            }
        }

        private void KnockBackFromPosition(float knockbackPower, Vector2 position)
        {
            var direction = ((Vector2)transform.position - position).normalized;
            var force = direction * knockbackPower;
            _knockbackController.Knockback(force);
        }

        private void TakeDamage(int amount)
        {
            _resources.GetResource(ResourceName.Health).Decrease(amount);
            Debug.Log("Damage is taken " + gameObject.name);
        }
    }
}