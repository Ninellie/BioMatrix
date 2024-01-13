using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameSession.Upgrades.Deck;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public interface ILevelUpDisplay
{
    string GetActiveDeckName();
    void ActivateNextDeck();
    void ActivatePreviousDeck();
    void DisplayRandomDecks(List<HandDeckData> decksData, int amount);
    void DestroyAllDecks();
}

public class ComplexLevelUpDisplay : MonoBehaviour, ILevelUpDisplay
{
    [SerializeField] private GameObject _deckPanelPrefab;
    [SerializeField] private Transform _decksArea;
    [SerializeField] private CardInfoUIPanel _cardInfoDisplay;
    [SerializeField] private ToggleGroup _cardsToggleGroup;

    private LinkedList<DeckPanel> _deckPanels;
    private LinkedListNode<DeckPanel> _activeDeck;

    public DeckPanel GetActiveDeck() => _activeDeck.Value;

    public string GetActiveDeckName() => _activeDeck.Value.GetName();

    public void ActivateDeck(DeckPanel deckPanel)
    {
        var deckPanelNode = _deckPanels.Find(deckPanel);
        if (deckPanelNode == _activeDeck) return;
        if (deckPanelNode == null)
        {
            Debug.LogError($"Trying to activate deck outside the array");
            return;
        }

        _activeDeck.Value.Deactivate();
        _activeDeck = deckPanelNode;
        _activeDeck.Value.Activate();
    }

    public void ActivateNextDeck()
    {
        if (_activeDeck.Next != null)
        {
            ActivateDeck(_activeDeck.Next.Value);
        }
        else
        {
            var deck = _activeDeck.Previous;
            if (deck == null) return;
            while (deck.Previous != null)
            {
                deck = deck.Previous;
            }

            ActivateDeck(deck.Value);
        }
    }

    public void ActivatePreviousDeck()
    {
        if (_activeDeck.Previous != null)
        {
            ActivateDeck(_activeDeck.Previous.Value);
        }
        else
        {
            var deck = _activeDeck.Next;
            if (deck == null) return;
            while (deck.Next != null)
            {
                deck = deck.Next;
            }

            ActivateDeck(deck.Value);
        }
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

    public void DestroyAllDecks()
    {
        _activeDeck = null;
        foreach (var deckPanel in _deckPanels)
        {
            deckPanel.gameObject.SetActive(false);
            Destroy(deckPanel.gameObject);
        }
    }

    private void DisplayDecks(List<HandDeckData> handData)
    {
        _cardInfoDisplay.SetHandData(handData);
        _deckPanels = new LinkedList<DeckPanel>();
        var middleDeckIndex = handData.Count / 2;
        var deckCount = 0;

        foreach (var deckData in handData)
        {
            var deckPanelGameObject = Instantiate(_deckPanelPrefab, _decksArea);
            var deckPanel = deckPanelGameObject.GetComponent<DeckPanel>();
            deckPanel.SetParentDisplay(this);
            deckPanel.SetCardInfo(_cardInfoDisplay);
            deckPanel.DisplayDeck(deckData.name, deckData.size, deckData.openedCardPosition, _cardsToggleGroup);
            deckPanel.UpdateContentPositionToOpenedCard();
            Debug.Log($"Displayed deck with name: {deckData.name}, size: {deckData.size}, opened card position in deck is: {deckData.openedCardPosition}");
            var deckNode = _deckPanels.AddLast(deckPanel);
            if (deckCount == middleDeckIndex) _activeDeck = deckNode;
            deckCount++;
        }

        _activeDeck.Value.Activate();

        var activeDeckName = _activeDeck.Value.GetName();
        _cardInfoDisplay.DisplayOpenedCardInfo(activeDeckName);
    }

    private List<HandDeckData> GetOpenedDecksDataFromList(IEnumerable<HandDeckData> decksData)
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