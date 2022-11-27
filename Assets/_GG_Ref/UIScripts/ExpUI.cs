using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpUI : MonoBehaviour
{
    public TMPro.TMP_Text expUI;
    public TMPro.TMP_Text lvlUI;

    private void Update()
    {
        var textLvl = "LVL: " + 
                      GameObject.FindGameObjectsWithTag("Player")[0]
                          .GetComponent<Player>()
                          .Level.ToString();

        var textExp = "exp to lvl up: " +
                      GameObject.FindGameObjectsWithTag("Player")[0]
                          .GetComponent<Player>()
                          .ExpToLvlup.ToString();

        lvlUI.text = textLvl;
        expUI.text = textExp;
    }
}
