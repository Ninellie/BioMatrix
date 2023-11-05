using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.GameSession.Camera
{
    public class MainCameraPlayerSeek : MonoBehaviour
    {
        private Transform _playerTransform;
        private Transform _mainCameraTransform;

        private void Start()
        {
            FindPlayer();
            _mainCameraTransform = UnityEngine.Camera.main.transform;
        }

        private void FixedUpdate()
        {
            SetMainCameraOnPlayer();
        }

        private void SetMainCameraOnPlayer()
        {
            if (_playerTransform == null)
            {
                FindPlayer();
                return;
            }
            _mainCameraTransform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, -100.0f);
        }

        private void FindPlayer()
        {
            if (FindObjectOfType<Player>() == null) return;
            _playerTransform = FindObjectOfType<Player>().transform;
        }
    }
}