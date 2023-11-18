using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.GameSession.UIScripts;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public class PlayerFirearm : MonoBehaviour
    {
        [SerializeField] private GameObject _ammo; // TODO convert to object pool
        [SerializeField] private PlayerTargetRuntimeSet _visibleEnemies;

        [SerializeField] private  AimMode _aimMode; // TODO Сделать ивент, который передаст bool переменную и подписать на неё соответствующий метод

        // TODO в том же компоненте есть 

        public GameEvent onShoot; // TODO В том же префабе оружия сделать EventListener который подпишется на этот ивент

        [SerializeField] private Reload reload;
        public bool CanShoot => _coolDownTimer <= 0
                                && magazine.Value > 0
                                && !reload.IsInProcess;

        [SerializeField] private IntReference magazine;

        private float _coolDownTimer;
        private PlayerTarget _currentTarget;

        private Transform _myTransform;

        public void SetAutoAim(bool value)
        {
            if (value)
            {
                _aimMode = AimMode.AutoAim;
                return;
            }
            _aimMode = AimMode.SelfAim;
        }

        private void Awake()
        {
            if (_myTransform == null) _myTransform = transform;
        }

        private void FixedUpdate()
        {
            _coolDownTimer -= Time.fixedDeltaTime;
        }

        private void TakeAsTarget(PlayerTarget target)
        {
            if (_currentTarget == target) return;
            _currentTarget.RemoveFromTarget();
            _currentTarget = target;
            _currentTarget.TakeAsTarget();
        }


        private Vector2 GetDirectionToNearestPlayerTarget()
        {
            var directionToNearestTarget = Vector2.zero;
            var distanceToNearestTarget = Mathf.Infinity;
            var targets = _visibleEnemies.items.ToArray();
            PlayerTarget nearestTarget = null;

            if (targets.Length > 0)
            {
                nearestTarget = targets[0];
            }

            foreach (var target in targets)
            {
                var distance = Vector2.Distance(_myTransform.position, target.transform.position);

                if (!(distance < distanceToNearestTarget)) continue;

                distanceToNearestTarget = distance;
                Vector2 direction = (target.transform.position - transform.position).normalized;
                directionToNearestTarget = direction;
                nearestTarget = target;
            }

            if (nearestTarget == null) return directionToNearestTarget;
            TakeAsTarget(nearestTarget);
            return directionToNearestTarget;
        }
    }
}