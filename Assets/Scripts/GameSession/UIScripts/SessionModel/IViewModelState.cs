namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    public interface IViewModelState
    {
        ViewModelStateType Name { get; }
        void Menu();
        void Resume();
        void Options();
        void LevelUp();
        void Win();
        void Lose();
    }
}