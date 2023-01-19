using UnityEngine;

namespace Assets.Scripts.Map
{
    public class MenuMapState : IMapState
    {
        public void Menu(Map map)
        {
            Resume(map);
        }
        public void Resume(Map map)
        {
            if (map.previousState == Map.activeState)
            {
                map.RememberPreviousState();
                map.SetActive();
                map.Unfreeze();
            }
            if (map.previousState == Map.levelUpState)
            {
                map.RememberPreviousState();
                map.SetLevelUp();
            }
            if (map.previousState == Map.gameEndState)
            {
                map.RememberPreviousState();
                map.SetGameEnd();
            }
            if (map.previousState == Map.optionsState)
            {
                map.RememberPreviousState();
                map.SetActive();
                map.Unfreeze();
            }
            map.CloseMenu();
        }
        public void Options(Map map)
        {
            map.RememberPreviousState();
            map.SetOptions();
            map.CloseMenu();
            map.OpenOptions();
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