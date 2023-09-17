using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameSession.Upgrades.Deck;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using Random = UnityEngine.Random;

public interface IDeckDisplay
{
    string GetActiveDeckName();
    void DisplayDecks(List<HandDeckData> decksData);
    void DisplayRandomDecks(List<HandDeckData> decksData, int amount);
}

public class DeckDisplay : MonoBehaviour, IDeckDisplay
{
    [SerializeField] private GameObject _deckPanelPrefab;
    [SerializeField] private Transform _decksArea;

    private LinkedList<DeckPanel> _deckPanels;

    private LinkedListNode<DeckPanel> _activeDeck;

    public string GetActiveDeckName()
    {
        return _activeDeck.Value.GetName();
    }

    public void ActiveDeckMoveNext()
    {
        _activeDeck = _activeDeck.Next;
    }

    public void ActiveDeckMovePrevious()
    {
        _activeDeck = _activeDeck.Previous;
    }

    public void DisplayDecks(List<HandDeckData> decksData)
    {
        _deckPanels = new LinkedList<DeckPanel>();

        foreach (var deckData in decksData)
        {
            var deckPanelGameObject = Instantiate(_deckPanelPrefab, _decksArea);
            var deckPanel = deckPanelGameObject.GetComponent<DeckPanel>();
            deckPanel.DisplayDeck(deckData.name, deckData.size, deckData.openedCardPosition);
            Debug.Log($"Displayed deck with name: {deckData.name}, size: {deckData.size}, opened card position in deck is: {deckData.openedCardPosition}");
            _deckPanels.AddLast(deckPanel);
        }

        var middleDeckIndex = decksData.Count / 2;
        var deckCount = 0;

        foreach (var deckPanel in _deckPanels)
        {
            if (deckCount == middleDeckIndex)
            {
                _activeDeck = new LinkedListNode<DeckPanel>(deckPanel);
            }
            deckCount++;
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