using UnityEngine;
public class MainCameraPlayerSeek : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(Lib2DMethods.PlayerPos.x, Lib2DMethods.PlayerPos.y, -100.0f);
    }
}
