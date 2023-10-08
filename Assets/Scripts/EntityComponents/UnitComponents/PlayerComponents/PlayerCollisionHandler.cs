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
        private void Awake()
        {
            _resources = GetComponent<ResourceList>();
            _invulnerability = GetComponent<Invulnerability>();
            _knockbackController = GetComponent<KnockbackController>();
            _movementController = GetComponent<PlayerMovementController>();
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            var otherTag = collision2D.collider.tag;
            var otherCollider = collision2D.otherCollider;

            switch (otherTag)
            {
                case "Enemy":
                    var enemy = collision2D.gameObject.GetComponent<Enemy>();
                    CollideWithEnemy(enemy);
                    break;
                case "Enclosure":
                    Debug.LogWarning("ENCLOSURE");
                    CollideWithEnclosure();
                    break;
                case "Cage":
                    Debug.LogWarning("Cage");
                    //CollideWithCage(collision2D);
                    break;
            }
        }

        private void DetectCollisionWithEnclosure()
        {
            // Если в следующем кадре, герой столкнётся с Enclosure, то оттолкнуть его в противоположную его движению сторону.
            // Получить вектор движения объекта
            // Кинуть луч по этому вектору
            // Понять, задел ли луч Enclosure
            // Если луч не задел Enclosure - выйти
            // Откинуть объект назад с определённой скоростью
        }

        private void CollideWithEnclosure()
        {

            // Оттолкнуться в противоположную движению сторону
            var direction = _movementController.GetRawMovementDirection();
            var force = direction * -50f;
            _knockbackController.Knockback(force);

            if (!_invulnerability.IsInvulnerable)
            {
                _invulnerability.ApplyInvulnerable();
                if (!_shield.Layers.IsEmpty)
                {
                    _shield.Layers.Decrease();
                }
                else
                {
                    //TakeDamage(1);
                }

                if (_resources.GetResource(ResourceName.Health).IsEmpty) return;
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


        //public void OnMove(InputValue input)
        //{
        //}
    }
}