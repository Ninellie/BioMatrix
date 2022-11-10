using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LVLUpManager : MonoBehaviour
{
    public GameObject lvlUpUI;
    public GameObject canvasUI;
    public TMPro.TMP_Text[] cardsText;

    private List<Card> selectedCards = new();
    private static ICardRepository cardRepository = new ArrayCardRepository();
    private readonly CardManager cardManager = new(cardRepository);

    private void OnEnable()
    {
        Player.OnLevelUp += InitiateLvlUp;
    }

    private void OnDisable()
    {
        Player.OnLevelUp -= InitiateLvlUp;
    }

    private void InitiateLvlUp()
    {
        canvasUI.GetComponent<PauseMenu>().PauseGame();
        DispleyThreeCards();
        lvlUpUI.SetActive(true);
    }

    private void DispleyThreeCards()
    {
        selectedCards = cardManager.GetDeck(3);

        for(int i = 0; i < cardsText.Length; i++)
        {
            cardsText[i].text = selectedCards[i].title;
        }
    }
    public void Improve(int index)
    {
        var key = selectedCards[index].improvement.Keys.First();
        var value = selectedCards[index].improvement.Values.First();
        Debug.Log(key);
        Debug.Log(value);

        switch (key)
        {
            case "movementSpeed":
                GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().movementSpeed += value;
                break;
            case "fireRate":
                //GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<FirearmSettings>().BulletsPerSecond += value;
                break;
            case "projectileSpeed":
                //GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<FirearmSettings>().ShootForce += value;
                break;
            case "pierceNumber":
                //GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<FirearmSettings>().PierceNumber += (int)value;
                break;
            case "reloadSpeed":
                //GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<FirearmSettings>().ReloadSpeed += value;
                break;
        }
    }
}
