using Assets.Scripts.Core.Render;
using Assets.Scripts.Core.Sets;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class PlayerTarget : MonoBehaviour
    {
        [SerializeField] private SpriteOutline _spriteOutline;
        [SerializeField] private Color _outlineColor;
        [SerializeField] private PlayerTargetRuntimeSet _runtimeSet;
        [field: SerializeField] public bool IsCurrent { get; private set; }
        public Transform Transform { get; private set; }

        private void Awake()
        {
            if (_spriteOutline == null) _spriteOutline = GetComponent<SpriteOutline>();
            if (Transform == null) Transform = transform;
        }

        private void OnBecameVisible()
        {
            _runtimeSet.Add(this);
        }

        private void OnBecameInvisible()
        {
            _runtimeSet.Remove(this);
            if (!IsCurrent) return;
            RemoveFromTarget();
        }

        public void TakeAsTarget()
        {
            _spriteOutline.SetColor(_outlineColor);
            _spriteOutline.SetShowOutline(true);
            IsCurrent = true;
        }

        public void RemoveFromTarget()
        {
            _spriteOutline.SetColor(_outlineColor);
            IsCurrent = false;
        }
    }
}