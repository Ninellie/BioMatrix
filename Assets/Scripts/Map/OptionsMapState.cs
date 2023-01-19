using UnityEngine;

namespace Assets.Scripts.Map
{
    public class OptionsMapState : IMapState
    {
        public void Menu(Map map)
        {
            map.RememberPreviousState();
            map.SetMenu();
            map.CloseOptions();
            map.OpenMenu();
        }
        public void Resume(Map map)
        {
            Menu(map);
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