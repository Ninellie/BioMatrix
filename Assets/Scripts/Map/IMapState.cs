namespace Assets.Scripts.Map
{
    public interface IMapState
    {
        MapState Name { get; }
        void Menu(Map map, IMapController mapController);
        void Resume(Map map, IMapController mapController);
        void Options(Map map, IMapController mapController);
        void LevelUp(Map map, IMapController mapController);
        void Win(Map map, IMapController mapController);
        void Lose(Map map, IMapController mapController);
    }
}