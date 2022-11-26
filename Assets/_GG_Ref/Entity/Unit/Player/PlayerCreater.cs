using UnityEngine;

public class PlayerCreater : MonoBehaviour
{
    public GameObject playerPrefab;

    private void OnEnable()
    {
        Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
    }
}
