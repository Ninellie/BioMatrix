using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class LVLUpManager : MonoBehaviour
{
    public GameObject lvlUpUI;
    public GameObject canvasUI;
    public TMPro.TMP_Text[] cardsText;

    private ArrayCardRepository cardRepository = new();
    private List<Card> selectedCards = new();

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
        CardsDisplay();
        lvlUpUI.SetActive(true);
    }

    private void CardsFill(List<Card> cardsList)
    {
        for (int i = 0; i < cardRepository.CardCount; i++)
        {
            Card card = cardRepository.Get(i);
            cardsList.Add(card);
        }
    }
    /// <summary>
    /// Takes 3 cards
    /// </summary>
    /// <returns>List of Cards with 3 different cards</returns>
    private List<Card> CardsIdentify()
    {
        List<Card> cardsList = new();

        CardsFill(cardsList);

        List<Card> selectedCards = new();

        for(int i = 0; i < 3; i++)
        {
            var rand = Random.Range(0, cardsList.Count);
            var card = cardsList[rand];
            selectedCards.Add(card);
            cardsList.RemoveAt(rand);
        }
        return selectedCards;
    }

    /// <summary>
    /// Displays 3 random cards on screen
    /// </summary>
    private void CardsDisplay()
    {
        selectedCards = CardsIdentify();

        for(int i = 0; i < cardsText.Length; i++)
        {
            cardsText[i].text = selectedCards[i].description;
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
            case "maximumHP":
                //GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().movementSpeed += value;
                break;
            case "shieldCount":

                break;
            case "regenerationRate":

                break;
            case "fireRate":
                GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<WeaponGun>().fireRate += value;
                break;
            case "projectileSpeed":
                GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<WeaponGun>().projectileSpeed += value;
                break;
            case "pierceNumber":
                GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<WeaponGun>().pierceNumber += (int)value;
                break;
            case "reloadSpeed":

                break;
            case "projectileNumber":

                break;
        }
    }
}
