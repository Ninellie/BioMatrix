namespace Assets.Scripts.Map
{
    public interface IViewController
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
}