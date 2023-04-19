using UnityEngine;

public class EventCntroller : MonoBehaviour
{
    [SerializeField] private float _cageAppearTime;
    [SerializeField] private Enclosure _enclosure;
    private TileBoxCreator _boxCreator;
    private GameTimer _cageTimer;

    void Awake()
    {
        _boxCreator = GetComponent<TileBoxCreator>();
    }
    void Start()
    {
        _cageTimer = new GameTimer(CageUp, _cageAppearTime);
    }
    void Update()
    {
        _cageTimer.Update();
    }
    private void CageUp()
    {
        _boxCreator.CreateBox();
        _enclosure.StartShrinking();
    }
}
