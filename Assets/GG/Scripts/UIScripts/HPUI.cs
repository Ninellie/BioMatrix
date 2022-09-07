using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    public TMPro.TMP_Text hpUI;

    // Update is called once per frame
    void Update()
    {
        string text = "HP: " + GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().lifePoints.ToString();
        hpUI.text = text;
    }
}
