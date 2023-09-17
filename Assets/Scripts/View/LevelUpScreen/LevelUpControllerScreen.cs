using Assets.Scripts.GameSession.Upgrades.Deck;
using UnityEngine;

public interface ILevelUpController
{
    void Initiate();
    void LevelUp();
    void SetHand(IHand hand);
}

public class LevelUpControllerScreen : MonoBehaviour, ILevelUpController
{
    [SerializeField] private int _baseDeckAmount = 3;
    private IDeckDisplay _deckDisplay;
    private IHand _hand;

    private void Awake()
    {
        _deckDisplay = gameObject.GetComponent<IDeckDisplay>();
    }

    public void Initiate()
    {
        var handDecks = _hand.GetHandData();
        _deckDisplay.DisplayRandomDecks(handDecks, _baseDeckAmount);
    }

    public void LevelUp()
    {
        var deckName = _deckDisplay.GetActiveDeckName();
        _hand.TakeCardFromDeck(deckName);
        _deckDisplay.DestroyAllDecks();
    }

    public void SetHand(IHand hand)
    {
        _hand = hand;
    }
}