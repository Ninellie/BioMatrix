using UnityEngine;

namespace Assets.Scripts.Map
{
    public class GameEndMapState : IMapState
    {
        public void Menu(Map map)
        {
            map.RememberPreviousState();
            map.SetMenu();
            map.OpenMenu();
        }
        public void Resume(Map map)
        {
            Debug.LogWarning("Massage");
        }
        public void Options(Map map)
        {
            Debug.LogWarning("Massage");
        }
        public void LevelUp(Map map)
        {
            Debug.LogWarning("Massage");
        }
        public void Win(Map map)
        {
            Debug.LogWarning("Massage");
        }
        public void Lose(Map map)
        {
            Debug.LogWarning("Massage");
        }
    }
}