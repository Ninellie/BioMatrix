using System;
using UnityEngine;

public class Reload : MonoBehaviour
{
    private float ReloadSpeed => GetComponent<FirearmSettings>().ReloadSpeed;
    private Magazine Magazine => GetComponent<Magazine>();

    [SerializeField] private GameObject _plateUi;
    public bool IsInProcess { get; private set; }

    private void Awake()
    {
        //plateUI = CreateDisabled(GetReloadPlate());
        _plateUi = GetReloadPlate();
    }
    private void OnEnable()
    {
        Magazine.OnEmpty += Initiate;
    }
    private void OnDisable()
    {
        Magazine.OnEmpty -= Initiate;
    }
    private void Initiate()
    {
        IsInProcess = true;
        _plateUi.SetActive(true);
        Invoke("Complete", 1 / ReloadSpeed);
    }
    private void Complete()
    {
        IsInProcess = false;
        _plateUi.SetActive(false);
        Magazine.FullFill();
    }
    private GameObject CreateDisabled(GameObject gameObject)
    {
        var createdGameObject = Instantiate(gameObject);
        createdGameObject.transform.SetParent(GameObject.FindWithTag("Canvas").transform, false);
        createdGameObject.SetActive(false);
        return createdGameObject;
    }
    private GameObject GetReloadPlate()
    {
        if (GameObject.FindWithTag("Canvas").GetComponent<ReloadPlate>().reloadPlate is null) throw new NotImplementedException();
        return GameObject.FindWithTag("Canvas").GetComponent<ReloadPlate>().reloadPlate;
    }
}
