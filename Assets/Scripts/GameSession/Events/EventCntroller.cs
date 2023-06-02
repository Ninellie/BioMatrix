using Assets.Scripts.Entity.Enclosure;
using UnityEngine;

namespace Assets.Scripts.GameSession.Events
{
    public class EventCntroller : MonoBehaviour
    {
        [SerializeField] private float _cageAppearTime;
        [SerializeField] private Enclosure _enclosure;
        private TileBoxCreator _boxCreator;
        private GameTimeScheduler _gameTimeScheduler;

        void Awake()
        {
            _boxCreator = GetComponent<TileBoxCreator>();
            _gameTimeScheduler = UnityEngine.Camera.main.GetComponent<GameTimeScheduler>();
        }
        void Start()
        {
            _gameTimeScheduler.Schedule(CageUp, _cageAppearTime);
        }
        private void CageUp()
        {
            _boxCreator.CreateBox();
            _enclosure.StartShrinking();
        }
    }
}
