using UnityEngine;

public class EventCntroller : MonoBehaviour
{
    //[SerializeField] private float[] _cageAppearTimes;
    [SerializeField] private float _cageAppearTime1;
    [SerializeField] private float _cageAppearTime2;
    [SerializeField] private float _cageAppearTime3;
    //[SerializeField] private GameObject _cageGameObject;
    //private GameTimer[] _cageTimers;
    private GameTimer _cageTimer1;
    private GameTimer _cageTimer2;
    private GameTimer _cageTimer3;


    private void Start()
    {
        //_cageTimers = new GameTimer[_cageTimers.Length];

        //for (int i = 0; i < _cageAppearTimes.Length; i++)
        //{
        //    _cageTimers[i] = new GameTimer(CageUp, _cageAppearTimes[i]);
        //}
        _cageTimer1 = new GameTimer(CageUp, _cageAppearTime1);
        _cageTimer2 = new GameTimer(CageUp, _cageAppearTime2);
        _cageTimer3 = new GameTimer(CageUp, _cageAppearTime3);
    }
    private void Update()
    {
        //foreach (var timer in _cageTimers)
        //{
        //    timer.Update();
        //}
        _cageTimer1.Update();
        _cageTimer2.Update();
        _cageTimer3.Update();
    }
    private void CageUp()
    {
        FindObjectOfType<Enclosure>(true).gameObject.SetActive(true);
        //_cageGameObject.SetActive(true);
    }
}
