using System.Linq;
using Assets.Scripts.GameSession.Upgrades.Deck;
using TMPro;
using UnityEngine;

public interface CardInfoDisplay
{
    void DisplayCardInfo(string deckName, int cardPosition);
    //void SetActiveCardInfo(string deckTitle, string cardTitle, string cardDescription);
    //void SetSelectedCardInfo(string deckTitle, string cardTitle, string cardDescription);
    //void Select();
    //void Deselect();
}

public class CardInfoUIPanel : MonoBehaviour, CardInfoDisplay
{
    private string _panelTitle;

    private TMP_Text _activeCardDeckTitle;
    private TMP_Text _activeCardTitle;
    private TMP_Text _activeCardDescription;

    [SerializeField] private IDeckRepository _deckRepository;
    [SerializeField] private EffectsRepository _effectsRepository;
    //private string activeCard
    //private string selectedCard
    public void DisplayCardInfo(string deckName, int cardPosition)
    {
        _activeCardDeckTitle.text = deckName;
        _activeCardTitle.text = $"Level {cardPosition}";
        _activeCardDescription.text = "";
        var decks = _deckRepository.GetDecks();
        var deck = decks.First(d => d.name.Equals(deckName));
        deck.cards.ToArray()[cardPosition].status = CardStatus.Obtained;
        var effectNames = deck.cards.ToArray()[cardPosition].effectNames;

        foreach (var effectName in effectNames)
        {
            var effect = _effectsRepository.GetEffectByName(effectName);
            _activeCardDescription.text += effect + "\r\n";
        }
    }
}