using UnityEngine;
public class MainCameraPlayerSeek : MonoBehaviour
{
    private bool IsPlayerExists => GameObject.FindObjectOfType<Player>() != null;
    private void Update()
    {
        if (IsPlayerExists)
            transform.position = new Vector3(Lib2DMethods.PlayerPosition.x, Lib2DMethods.PlayerPosition.y, -100.0f);
    }
}