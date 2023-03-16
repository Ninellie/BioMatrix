using UnityEngine;

public class EventCntroller : MonoBehaviour
{
    [SerializeField] private float _cageAppearTime;
    [SerializeField] private GameObject _cageGameObject;
    private GameTimer _cageTimer;
    
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
        _cageGameObject.SetActive(true);
    }
}
