namespace Assets.Scripts.Map
{
    public interface IMapState
    {
        void Menu(Map map);
        void Resume(Map map);
        void Options(Map map);
        void LevelUp(Map map);
        void Win(Map map);
        void Lose(Map map);
    }
}