namespace UIScripts.SessionModel
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
        void OpenLoseScreen();
        void OpenWinScreen();
        void InitiateLevelUp();
        void CloseLevelUp();
        void InitiateMutation();
        void CloseMutationScreen();
    }
}