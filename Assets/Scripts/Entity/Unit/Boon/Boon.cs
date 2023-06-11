using Assets.Scripts.Entity.Unit.Enemy;
using Assets.Scripts.Entity.Unit.Player;
using UnityEngine;

namespace Assets.Scripts.Entity.Unit.Boon
{
    [RequireComponent(typeof(UnitStatsSettings))]
    public class Boon : Unit
    {
        public UnitStatsSettings Settings => GetComponent<UnitStatsSettings>();

        private const float SpeedConst = 1500;
        private readonly Rarity _rarity = new();

        private void Awake() => BaseAwake(Settings);

        private void OnEnable() => BaseOnEnable();

        private void OnDisable() => BaseOnDisable();

        private void OnTriggerStay2D(Collider2D otherCollider2D) => BaseOnTriggerStay2D(otherCollider2D);

        private void OnTriggerEnter2D(Collider2D otherCollider2D) => BaseOnTriggerEnter2D(otherCollider2D);

        private void Update() => BaseUpdate();
        
        protected override void BaseAwake(UnitStatsSettings settings)
        {
            Debug.Log($"{gameObject.name} Boon Awake");
            base.BaseAwake(settings);

            _rarity.Value = RarityEnum.Normal;
        }

        private void BaseOnTriggerEnter2D(Collider2D collider2D)
        {
            var collisionGameObject = collider2D.gameObject;

            if (!collider2D.gameObject.CompareTag("Player")) return;
            if (collider2D is not BoxCollider2D) return;
            Debug.Log("The exp crystal was taken");
            var player = collisionGameObject.GetComponent<Player.Player>();
            var expGiven = LifePoints.GetMaxValue();
            player.GetExperience(expGiven);
            TakeDamage(LifePoints.GetValue());
        }

        private void BaseOnTriggerStay2D(Collider2D collider2D)
        {
            if (!collider2D.gameObject.CompareTag("Player")) return;
            Vector2 nextPosition = transform.position;
            Vector2 direction = (collider2D.transform.position - transform.position).normalized;
            var movementVelocity = direction * SpeedConst;
            nextPosition += movementVelocity * Time.fixedDeltaTime;

            Rb2D.MovePosition(nextPosition);
        }
    }
}
