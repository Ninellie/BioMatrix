using System;
using UnityEngine;

public class Reload : MonoBehaviour
{
    public bool IsInProcess { get; private set; }

    [SerializeField] private GameObject _plateUi;
    private Firearm _firearm;
    [SerializeField] private GameTimeScheduler _gameTimeScheduler;

    private void Awake()
    {
        _firearm = GetComponent<Firearm>();
        _gameTimeScheduler = Camera.main.GetComponent<GameTimeScheduler>();

        if (_firearm.GetIsForPlayer())
        {
            _plateUi = FindObjectOfType<ReloadPlate>().reloadPlate;
        }
    }
    
    private void OnEnable()
    {
        _firearm.Magazine.onEmpty += Initiate;
    }

    private void OnDisable()
    {
        _firearm.Magazine.onEmpty -= Initiate;
    }

    private void Initiate()
    {
        IsInProcess = true;

        if (_firearm.GetIsForPlayer())
        {
            _plateUi.SetActive(true);
        }
        
        _gameTimeScheduler.Schedule(Complete, 1f / _firearm.ReloadSpeed.Value);

        //_firearm.OnReload();
    }

    private void Complete()
    {
        IsInProcess = false;

        if (_firearm.GetIsForPlayer())
        {
            _plateUi.SetActive(false);
        }
        
        _firearm.Magazine.Fill();

        _firearm.OnReloadEnd();
    }
}
