using System.Collections.Generic;
using Assets.Scripts.GameSession.Upgrades.Deck;
using UnityEngine;

public interface IDeckDisplay
{
    string GetActiveDeckName();
    void DisplayDecks(List<HandDeckData> decksData);
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
}