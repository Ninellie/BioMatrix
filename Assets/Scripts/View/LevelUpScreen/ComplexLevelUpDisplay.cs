using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameSession.Upgrades.Deck;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public interface ILevelUpDisplay
{
    void DisplayDecks(List<HandDeckData> handData);
    void DestroyAllDecks();
    string GetActiveDeckName();
    void DisplayRandomDecks(List<HandDeckData> decksData, int amount);
    void ActivateNextDeck();
    void ActivatePreviousDeck();
    void SelectNextCard();
    void SelectPreviousCard();
}

public class ComplexLevelUpDisplay : MonoBehaviour, ILevelUpDisplay
{
    [SerializeField] private GameObject _deckPanelPrefab;
    [SerializeField] private Transform _decksArea;
    [SerializeField] private CardInfoUIPanel _cardInfoDisplay;
    [SerializeField] private ToggleGroup _cardsToggleGroup;

    private LinkedList<DeckPanel> _deckPanels;
    private LinkedListNode<DeckPanel> _activeDeck;

    public void DestroyAllDecks()
    {
        foreach (var deckPanel in _deckPanels)
        {
            deckPanel.gameObject.SetActive(false);
            Destroy(deckPanel.gameObject);
        }
    }

    public string GetActiveDeckName()
    {
        return _activeDeck.Value.GetName();
    }

    public void ActivateNextDeck()
    {
        if (_activeDeck.Next is null)
        {
            Debug.LogWarning($"Next deck does not exist");
            return;
        }
        _activeDeck.Value.Deactivate();
        _activeDeck = _activeDeck.Next;
        _activeDeck.Value.Activate();
        //UpdateContentPosition();
        //DisplayActiveCardInfo();
    }

    public void ActivatePreviousDeck()
    {
        if (_activeDeck.Previous is null)
        {
            Debug.LogWarning($"Previous deck does not exist");
            return;
        }
        _activeDeck.Value.Deactivate();
        _activeDeck = _activeDeck.Previous;
        _activeDeck.Value.Activate();
        //UpdateContentPosition();
        //DisplayActiveCardInfo();
    }

    public void SelectNextCard()
    {
        //_activeDeck.Value.SelectNextCard();
    }

    public void SelectPreviousCard()
    {
        //_activeDeck.Value.SelectPreviousCard();
    }

    public void DisplayDecks(List<HandDeckData> handData)
    {
        _cardInfoDisplay.SetHandData(handData);
        _deckPanels = new LinkedList<DeckPanel>();
        var middleDeckIndex = handData.Count / 2;
        var deckCount = 0;

        foreach (var deckData in handData)
        {
            var deckPanelGameObject = Instantiate(_deckPanelPrefab, _decksArea);
            var deckPanel = deckPanelGameObject.GetComponent<DeckPanel>();
            deckPanel.SetCardInfo(_cardInfoDisplay);
            deckPanel.DisplayDeck(deckData.name, deckData.size, deckData.openedCardPosition, _cardsToggleGroup);
            //deckPanel.UpdateContentPosition();
            deckPanel.UpdateContentPositionToOpenedCard();
            Debug.Log($"Displayed deck with name: {deckData.name}, size: {deckData.size}, opened card position in deck is: {deckData.openedCardPosition}");
            var deckNode = _deckPanels.AddLast(deckPanel);
            if (deckCount == middleDeckIndex) _activeDeck = deckNode;
            deckCount++;
        }

        _activeDeck.Value.Activate();

        var activeDeckName = _activeDeck.Value.GetName();
        //var activeCardIndex = _activeDeck.Value.GetSelectedCardIndex();
        //_cardInfoDisplay.DisplayCardInfo(activeDeckName, activeCardIndex);
        _cardInfoDisplay.DisplayOpenedCardInfo(activeDeckName);
    }

    public void DisplayRandomDecks(List<HandDeckData> decksData, int amount)
    {
        var trueAmount = Mathf.Clamp(amount, 0, decksData.Count);
        var decksDataTemp = GetOpenedDecksDataFromList(decksData);
        var selectedDecks = new List<HandDeckData>(trueAmount);

        for (int i = 0; i < selectedDecks.Capacity; i++)
        {
            var deck = GetRandomDeckDataFromList(decksDataTemp);
            selectedDecks.Add(deck);
            decksDataTemp.Remove(deck);
        }

        DisplayDecks(selectedDecks);
    }

    public List<HandDeckData> GetOpenedDecksDataFromList(List<HandDeckData> decksData)
    {
        var decksWithOpenedCards = decksData.Where(deckData => deckData.openedCardPosition < deckData.size).ToList();
        return decksWithOpenedCards;
    }

    private HandDeckData GetRandomDeckDataFromList(List<HandDeckData> decksData)
    {
        var sum = decksData.Sum(x => x.dropWeight);
        var next = Random.Range(0, sum);
        var limit = 0f;

        foreach (var deck in decksData)
        {
            limit += deck.dropWeight;
            if (next < limit)
            {
                return deck;
            }
        }

        Debug.LogWarning("Error, mb all weights of decks is 0");
        throw new InvalidOperationException("");
    }
}