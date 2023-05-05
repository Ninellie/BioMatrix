using System;
using UnityEngine;

public class Reload : MonoBehaviour
{
    private float ReloadSpeed => _firearm.ReloadSpeed.Value;
    private Firearm _firearm;
    private Magazine _magazine;

    [SerializeField] private GameObject _plateUi;
    public bool IsInProcess { get; private set; }
    private void Awake()
    {
        _firearm = GetComponent<Firearm>();
        _magazine = GetComponent<Magazine>();
        if (_firearm.GetIsForPlayer())
        {
            _plateUi = GetReloadPlate();
        }
    }
    private void OnEnable()
    {
        _magazine.onEmpty += Initiate;
    }
    private void OnDisable()
    {
        _magazine.onEmpty -= Initiate;
    }
    private void Initiate()
    {
        IsInProcess = true;

        if (_firearm.GetIsForPlayer())
        {
            _plateUi.SetActive(true);
        }

        Invoke(nameof(Complete), 1 / ReloadSpeed);

        _firearm.OnRecharge();
    }
    private void Complete()
    {
        IsInProcess = false;

        if (_firearm.GetIsForPlayer())
        {
            _plateUi.SetActive(false);
        }
        
        _magazine.FullFill();

        _firearm.OnRechargeEnd();
    }
    private GameObject GetReloadPlate()
    {
        if (GameObject.FindWithTag("Canvas").GetComponent<ReloadPlate>().reloadPlate is null) throw new NotImplementedException();
        return GameObject.FindWithTag("Canvas").GetComponent<ReloadPlate>().reloadPlate;
    }
}
