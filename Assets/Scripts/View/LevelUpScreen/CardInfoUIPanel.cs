using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameSession.Upgrades.Deck;
using TMPro;
using UnityEngine;

public class CardInfoUIPanel : MonoBehaviour
{
    [SerializeField] private PatternDeckRepository _deckRepository;
    [Header("Text components")]
    [SerializeField] private TMP_Text _activeCardDeckTitle;
    [SerializeField] private TMP_Text _activeCardTitle;
    [SerializeField] private TMP_Text _activeCardDescription;

    private List<HandDeckData> _handData;

    public void SetHandData(List<HandDeckData> decksData)
    {
        _handData = decksData;
    }

    public void DisplayOpenedCardInfo(string deckName)
    {
        var cardPosition = _handData.Where(data => data.name == deckName).Select(data => data.openedCardPosition).FirstOrDefault();
        DisplayCardInfo(deckName, cardPosition);
    }

    public void DisplayCardInfo(string deckName, int cardPosition)
    {
        _activeCardDeckTitle.text = deckName;
        _activeCardTitle.text = $"Level {cardPosition + 1}";
        _activeCardDescription.text = "";
        var decks = _deckRepository.Decks;
        var deck = decks.First(d => d.name.Equals(deckName));
        _activeCardDescription.text = deck.cards.ToArray()[cardPosition].description;
    }
}