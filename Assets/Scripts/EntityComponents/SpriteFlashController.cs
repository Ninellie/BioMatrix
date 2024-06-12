using UnityEngine;

namespace EntityComponents
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteFlashController : MonoBehaviour
    {
        [Header("Inner components")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _originalColor;
        [Header("Settings")]
        [SerializeField] private float _baseFlashingDuration = 1f;
        [SerializeField] private float _flashDuration = 0.1f;
        [SerializeField] private float _flashAlpha = 0.3f;
        [Header("Indicators")]
        [SerializeField] private float _flashingStopwatch;
        [SerializeField] private bool _isFlashing = false;

        private void Awake()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
            _isFlashing = false;
        }

        private void Update()
        {
            if (!_isFlashing) return;
            _flashingStopwatch -= Time.deltaTime;
            var t = Mathf.PingPong(_flashingStopwatch / _flashDuration, 1f);
            var alpha = Mathf.Lerp(1f, _flashAlpha, t);
            _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
        }

        [ContextMenu("Start flashing", false, 51)]
        public void StartFlashing()
        {
            _isFlashing = true;
            _flashingStopwatch = _baseFlashingDuration;
            Invoke(nameof(StopFlashing), _baseFlashingDuration);
        }

        public void StopFlashing()
        {
            _isFlashing = false;
            _spriteRenderer.color = Color.white;
            _flashingStopwatch = 0f;
        }
    }
}