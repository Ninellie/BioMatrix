using Assets.Scripts.GameSession.UIScripts.SessionModel;
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
    [SerializeField] private IDeckDisplay _deckDisplay;
    [SerializeField] private GameSessionController _gameSessionController;
    private IHand _hand;

    private void Awake()
    {
        _deckDisplay = gameObject.GetComponent<IDeckDisplay>();
    }

    public void Initiate()
    {
        var hand = _hand.GetHandData();
        _deckDisplay.DisplayDecks(hand);
    }

    public void LevelUp()
    {
        var deckName = _deckDisplay.GetActiveDeckName();
        _hand.TakeCardFromDeck(deckName);
        _gameSessionController.Resume();
    }

    public void SetHand(IHand hand)
    {
        _hand = hand;
    }
}