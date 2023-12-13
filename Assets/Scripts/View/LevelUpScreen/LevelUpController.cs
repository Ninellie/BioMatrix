using Assets.Scripts.GameSession.Upgrades.Deck;
using UnityEngine;

public interface ILevelUpController
{
    void Initiate();
    void LevelUp();
}

public class LevelUpController : MonoBehaviour, ILevelUpController
{
    [SerializeField] private int _baseDeckAmount = 3;
    private ILevelUpDisplay _levelUpDisplay;
    [SerializeField] private Hand _hand;

    private void Awake()
    {
        _levelUpDisplay = gameObject.GetComponent<ILevelUpDisplay>();
    }

    public void Initiate()
    {
        var handDecks = _hand.GetHandData();
        _levelUpDisplay.DisplayRandomDecks(handDecks, _baseDeckAmount);
    }

    public void LevelUp()
    {
        var deckName = _levelUpDisplay.GetActiveDeckName();
        _hand.TakeCardFromDeck(deckName);
        _levelUpDisplay.DestroyAllDecks();
    }
}