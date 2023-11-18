using Assets.Scripts.Core.Render;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class PlayerTarget : MonoBehaviour
    {
        [SerializeField] private SpriteOutline _spriteOutline;
        [SerializeField] private Color _targetOutlineColor;
        [SerializeField] private PlayerTargetRuntimeSet _runtimeSet;

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
        }

        public void TakeAsTarget()
        {
            _spriteOutline.SetColor(_targetOutlineColor);
            _spriteOutline.SetShowOutline(true);
        }

        public void RemoveFromTarget()
        {
            _spriteOutline.SetColor(_targetOutlineColor);
        }
    }
}