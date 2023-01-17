using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Map
{
    public class SimpleMapController : IMapController
    {
        private PlayerInput _playerInput;
        private GameTimer _gameTimer;
        private GameObject _menu;
        private GameObject _options;
        private LevelUp _levelUp;

        public SimpleMapController(GameTimer gameTimer, GameObject menu, PlayerInput playerInput, LevelUp levelUp)
        {
            _gameTimer = gameTimer;
            _menu = menu;
            _playerInput = playerInput;
            _levelUp = levelUp;
        }
        public void Freeze()
        {
            Time.timeScale = 0f;
            _gameTimer.Stop();
        }
        public void Unfreeze()
        {
            Time.timeScale = 1f;
            _gameTimer.Resume();
        }
        public void OpenMenu()
        {
            _menu.enabled = true;
        }

        public void CloseMenu()
        {
            _menu.enabled = false;
        }

        public void OpenOptions()
        {
            _options.gameObject.SetActive(true);
        }

        public void CloseOptions()
        {
            throw new System.NotImplementedException();
        }

        public void OpenWinScreen()
        {
            throw new System.NotImplementedException();
        }

        public void OpenLoseScreen()
        {
            throw new System.NotImplementedException();
        }

        public void CloseLevelUp()
        {
            throw new System.NotImplementedException();
        }

        public void InitiateLevelUp()
        {
            throw new System.NotImplementedException();
        }
    }
    public interface IMapController
    {
        void Freeze();
        void Unfreeze();
        void OpenMenu();
        void CloseMenu();
        void OpenOptions();
        void CloseOptions();
        void OpenWinScreen();
        void OpenLoseScreen();
        void CloseLevelUp();
        void InitiateLevelUp();
    }
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
    public interface IMapState
    {
        void Menu(Map map);
        void Resume(Map map);
        void Options(Map map);
        void LevelUp(Map map);
        void Win(Map map);
        void Lose(Map map);
    }
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
    public class MenuMapState : IMapState
    {
        public void Menu(Map map)
        {
            Debug.LogWarning("Massage");
        }
        public void Resume(Map map)
        {
            if (map.IsPreviousActive())
            {
                map.RememberPreviousState();
                map.SetActive();
                map.Unfreeze();
            }
            if (map.IsPreviousLevelUpOpen())
            {
                map.RememberPreviousState();
                map.SetLevelUp();
            }
            if (map.IsPreviousEndGameScreenOpen())
            {
                map.RememberPreviousState();
                map.SetGameEnd();
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
    public class LevelUpMapState : IMapState
    {
        public void Menu(Map map)
        {
            map.RememberPreviousState();
            map.SetMenu();
            map.OpenMenu();
        }
        public void Resume(Map map)
        {
            map.RememberPreviousState();
            map.SetActive();
            map.CloseLevelUp();
            map.Unfreeze();
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