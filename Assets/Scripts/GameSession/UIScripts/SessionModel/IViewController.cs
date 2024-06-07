namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    public interface IViewController
    {
        void Freeze();
        void Unfreeze();
        void Repulse();
        void OpenPauseScreen();
        void ClosePauseScreen();
        void CloseStartScreen();
        void OpenOptions();
        void CloseOptions();
        void OpenWinScreen();
        void OpenLoseScreen();
        void CloseLevelUp();
        void InitiateLevelUp();
    }
}