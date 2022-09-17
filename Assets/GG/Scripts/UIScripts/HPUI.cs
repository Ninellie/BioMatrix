using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    public TMPro.TMP_Text hpUI;
    public TMPro.TMP_Text ammoUI;

    // Update is called once per frame
    void Update()
    {
        string textHP = "HP: " + GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().lifePoints.ToString();
        string textAmmo = "AMMO: " + GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<Revolver>().magazineCurrent.ToString();

        hpUI.text = textHP;
        ammoUI.text = textAmmo;
    }
}
