namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    public interface IViewController
    {
        void Freeze();
        void Unfreeze();
        void OpenMenu();
        void CloseMenu();
        void CloseStartScreen();
        void OpenOptions();
        void CloseOptions();
        void OpenWinScreen();
        void OpenLoseScreen();
        void CloseLevelUp();
        void InitiateLevelUp();
    }
}