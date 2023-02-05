namespace Assets.Scripts.Map
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