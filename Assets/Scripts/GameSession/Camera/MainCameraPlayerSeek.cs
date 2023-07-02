using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.GameSession.Camera
{
    public class MainCameraPlayerSeek : MonoBehaviour
    {
        public Vector2 PlayerPosition
        {
            get
            {
                if (!IsPlayerExists) return Vector2.zero;
                Vector2 playerPos = GameObject.FindObjectOfType<Player>().transform.position;
                return playerPos;
            }
        }
        private bool IsPlayerExists => GameObject.FindObjectOfType<Player>() != null;
        private void Update()
        {
            if (IsPlayerExists)
                transform.position = new Vector3(PlayerPosition.x, PlayerPosition.y, -100.0f);
        }
    }
}