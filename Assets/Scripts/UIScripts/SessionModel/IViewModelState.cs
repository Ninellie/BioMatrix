namespace UIScripts.SessionModel
{
    public interface IViewModelState
    {
        ViewModelStateType Name { get; }
        void PauseScreen();
        void Resume();
        void Options();
        void LevelUp();
        void Mutate();
        void Win();
        void Lose();
    }
}