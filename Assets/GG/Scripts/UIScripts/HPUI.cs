using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    public TMPro.TMP_Text hpUI;
    public TMPro.TMP_Text ammoUI;

    // Update is called once per frame
    private void Update()
    {
        var textHP = "HP: " + GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().CurrentLifePoints.ToString();
        var textAmmo = "AMMO: " + GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<Magazine>().CurrentAmount.ToString();

        hpUI.text = textHP;
        ammoUI.text = textAmmo;
    }
}
