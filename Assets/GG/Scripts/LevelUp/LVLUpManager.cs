using System.Collections.Generic;
using UnityEngine;

public class LVLUpManager : MonoBehaviour
{
    
    public GameObject lvlUpUI;
    public GameObject canvasUI;
    public TMPro.TMP_Text[] cardsText;

    private List<Card> _selectedCards = new();
    private static ICardRepository _cardRepository = new ArrayCardRepository();
    private readonly CardManager _cardManager = new(_cardRepository);

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
        DisplayThreeCards();
        lvlUpUI.SetActive(true);
    }

    private void DisplayThreeCards()
    {
        _selectedCards = _cardManager.GetDeck(3);

        for(int i = 0; i < cardsText.Length; i++)
        {
            cardsText[i].text = _selectedCards[i].Title;
        }
    }
    public void Improve(int index)
    {
        foreach (var modifier in _selectedCards[index].ModifierList)
        {
            var key = modifier.ParameterName;
            var value = modifier.Value;
            //var key = _selectedCards[index].improvement.Keys.First();
            //var value = _selectedCards[index].improvement.Values.First();
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
}
