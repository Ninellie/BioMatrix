using Assets.Scripts.Core.Render;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class PlayerTarget : MonoBehaviour
    {
        [SerializeField] private SpriteOutline _spriteOutline;
        [SerializeField] private Color _targetOutlineColor;

        private void Awake()
        {
            if (_spriteOutline == null)
            {
                _spriteOutline = GetComponent<SpriteOutline>();
            }
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