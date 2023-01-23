namespace Assets.Scripts.Map
{
    public interface IMapState
    {
        MapStateType Name { get; }
        void Menu();
        void Resume();
        void Options();
        void LevelUp();
        void Win();
        void Lose();
    }
}