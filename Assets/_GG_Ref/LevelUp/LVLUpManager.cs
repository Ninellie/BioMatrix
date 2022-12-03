using System.Collections.Generic;
using UnityEngine;

public class LVLUpManager : MonoBehaviour
{
    public GameObject lvlUpUI;
    public GameObject canvasUI;
    public TMPro.TMP_Text[] cardsText;

    private List<Card> _selectedCards = new();
    private static readonly ICardRepository CardRepository = new ArrayCardRepository();
    private readonly CardManager _cardManager = new(CardRepository);

    //private void OnEnable()
    //{
    //    FindObjectOfType<Camera>()
    //        .GetComponent<PlayerCreator>().onPlayerCreated += Subscription;
    //}
    //private void OnDisable()
    //{
    //    FindObjectOfType<Camera>()
    //        .GetComponent<PlayerCreator>().onPlayerCreated -= Subscription;
    //}
    public void Subscription()
    {
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onLevelUp += InitiateLvlUp;

        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onLevelUp -= InitiateLvlUp;

        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath -= Unsubscription;
    }
    private void InitiateLvlUp()
    {
        canvasUI.GetComponent<PauseMenu>().PauseGame();
        DisplayThreeCards();
        lvlUpUI.SetActive(true);
    }
    private void DisplayThreeCards()
    {
        _selectedCards = _cardManager.GetDeck(3);

        for(var i = 0; i < cardsText.Length; i++)
        {
            cardsText[i].text = _selectedCards[i].Title;
        }
    }
    public void Improve(int i)
    {
        for (i = 0; i < _selectedCards[i].InfluencedStats.Length; i++)
        {
            var influencedStat = _selectedCards[i].InfluencedStats[i];
            var value = _selectedCards[i].ModifierList[i].Value;
            Debug.Log(influencedStat);
            Debug.Log(value);

            switch (influencedStat)
            {
                case "speed":
                    //GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().speed += value;
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
}
