using UnityEngine;

namespace Assets.Scripts.Map
{
    public class Map
    {   
        internal static IMapState optionsState = new OptionsMapState();
        internal static IMapState activeState = new ActiveMapState();
        internal static IMapState menuState = new MenuMapState();
        internal static IMapState gameEndState = new GameEndMapState();
        internal static IMapState levelUpState = new LevelUpMapState();
        internal IMapState state = activeState;
        internal IMapState previousState = activeState;
        private IMapController _mapController;
        public Map(IMapController mapController)
        {
            _mapController = mapController;
        }

        public void Menu() { state.Menu(this); }
        public void Resume() { state.Resume(this); }
        public void Options() { state.Options(this); }
        public void LevelUp() { state.LevelUp(this); }
        public void Win() { state.Win(this); }
        public void Lose() { state.Lose(this); }

        internal void SetActive() { state = activeState; }
        internal void SetMenu() { state = menuState; }
        internal void SetLevelUp() { state = levelUpState; }
        internal void SetOptions() { state = optionsState; }
        internal void SetGameEnd() { state = gameEndState; }

        internal void RememberPreviousState() { previousState = state; }
        internal bool IsPreviousActive() { return previousState == activeState; }
        internal bool IsPreviousMenuOpen() { return previousState == menuState; }
        internal bool IsPreviousLevelUpOpen() { return previousState == levelUpState; }
        internal bool IsPreviousOptionsOpen() { return previousState == optionsState; }
        internal bool IsPreviousEndGameScreenOpen() { return previousState == gameEndState; }

        internal bool IsActive() { return state == activeState; }
        internal bool IsMenuOpen() { return state == menuState; }
        internal bool IsLevelUpOpen() { return state == levelUpState; }
        internal bool IsOptionsOpen() { return state == optionsState; }
        internal bool IsEndGameScreenOpen() { return state == gameEndState; }

        internal void Freeze() { _mapController.Freeze(); }
        internal void Unfreeze() { _mapController.Unfreeze(); }
        internal void OpenMenu() { _mapController.OpenMenu(); }
        internal void CloseMenu() { _mapController.CloseMenu(); }
        internal void OpenOptions() { _mapController.OpenOptions(); }
        internal void CloseOptions() { _mapController.CloseOptions(); }
        internal void WinGame() { _mapController.OpenWinScreen(); }
        internal void LoseGame() { _mapController.OpenLoseScreen(); }
        internal void InitiateLevelUp() { _mapController.InitiateLevelUp(); }
        internal void CloseLevelUp() { _mapController.CloseLevelUp(); }
    }
}