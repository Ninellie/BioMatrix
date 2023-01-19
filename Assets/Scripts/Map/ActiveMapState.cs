using UnityEngine;

namespace Assets.Scripts.Map
{
    public class ActiveMapState : IMapState
    {
        public void Menu(Map map)
        {
            map.RememberPreviousState();
            map.SetMenu();
            map.Freeze();
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
            map.RememberPreviousState();
            map.SetLevelUp();
            map.Freeze();
            map.InitiateLevelUp();
        }
        public void Win(Map map)
        {
            map.RememberPreviousState();
            map.SetGameEnd();
            map.Freeze();
            map.WinGame();
        }
        public void Lose(Map map)
        {
            map.RememberPreviousState();
            map.SetGameEnd();
            map.Freeze();
            map.LoseGame();
        }
    }
}