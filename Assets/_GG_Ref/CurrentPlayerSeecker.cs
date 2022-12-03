using UnityEngine;

public static class CurrentPlayerSeecker
{
    public static GameObject CurrentPlayer { get; private set; }
    
    public static void SetCurrentPlayer(GameObject player)
    {
        CurrentPlayer = player;
    }
}
