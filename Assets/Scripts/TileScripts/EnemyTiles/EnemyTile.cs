using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.TileScripts.EnemyTiles
{
    public class EnemyTile : MonoBehaviour
    {
        [Header("Settings")]

        [SerializeField]
        [Tooltip("In minutes")]
        [Range(0, 5)] private float _activationTime;
    
        [SerializeField]
        [Tooltip("In minutes")]
        [Range(0, 5)] private float _activationDuration;
    
        [SerializeField]
        [Tooltip("Time in seconds after activation before it starts to become damaging")]
        [Range(0, 30)] private float _activationDelay;

        [SerializeField]
        [Tooltip("In seconds")]
        [Range(0, 1)] private float _blinkingTime;
    
        [SerializeField]
        [Tooltip("In seconds")]
        [Range(0, 1)] private float _blinkTime;

        [SerializeField] [ColorUsage(true)] private Color _activatedColor;
        [SerializeField] [ColorUsage(true)] private Color _deactivatedColor;
        [SerializeField] [ColorUsage(true)] private Color _blinkingColor;

        [Header("Indicators")]
        [SerializeField] private bool _isCharging;
        [SerializeField] private bool _isBlinking;
        [SerializeField] private float _alphaPerSecondCharging;

        private TilemapRenderer _tilemapRenderer;
        private Rigidbody2D _rb;
        private float _blinkTimer;

        #region UnityMassages

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _tilemapRenderer = GetComponent<TilemapRenderer>();
            _isCharging = false;
            Deactivate();
        }

        private void Update()
        {
            if (_isCharging)
            {
                if (_activatedColor.a > _tilemapRenderer.material.color.a)
                {
                    var color = new Color(1, 1, 1,
                        _tilemapRenderer.material.color.a + _alphaPerSecondCharging * Time.deltaTime);

                    _tilemapRenderer.material.color = color;
                }
            }

            if (!_isBlinking) return;
            _blinkTimer += Time.deltaTime;
            var t = Mathf.PingPong(_blinkTimer / _blinkTime, 1f);
            var alpha = Mathf.Lerp(1f, _blinkingColor.a, t);
            _tilemapRenderer.material.color = new Color(_blinkingColor.r, _blinkingColor.g, _blinkingColor.b, alpha);
        }

        private void Start()
        {
            StartCoroutine(InitActivation());
        }

        #endregion

        private IEnumerator InitActivation()
        {
            yield return new WaitForSeconds(_activationTime * 60);
            _alphaPerSecondCharging = _activatedColor.a - _deactivatedColor.a;
            _alphaPerSecondCharging /= _activationDelay;
            _isCharging = true;
            yield return new WaitForSeconds(_activationDelay);
            _isCharging = false;
            _isBlinking = true;
            yield return new WaitForSeconds(_blinkingTime);
            _isBlinking = false;
            Activate();
            yield return new WaitForSeconds(_activationDuration * 60);
            Deactivate();
        }

        private void Activate()
        {
            _tilemapRenderer.material.color = _activatedColor;
            _rb.simulated = true;
        }

        private void Deactivate()
        {
            _tilemapRenderer.material.color = _deactivatedColor;
            _rb.simulated = false;
        }
    }
}