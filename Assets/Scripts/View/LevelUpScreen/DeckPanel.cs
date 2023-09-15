using System.Collections.Generic;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.GameSession.Upgrades.Deck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface ILevelUpController
{
    void Initiate();
    void LevelUp(); // Берёт активную карту и применяет на игрока
    void AcceptActiveDeck();
}

public class LevelUpControllerScreen : MonoBehaviour, ILevelUpController
{
    [SerializeField] private GameObject deck;
    public void Initiate()
    {
        throw new System.NotImplementedException();
    }

    public void LevelUp()
    {
        throw new System.NotImplementedException();
    }

    public void AcceptActiveDeck()
    {
        throw new System.NotImplementedException();
    }
}

public class DeckDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject DeckPanelPrefab;

    public void DisplayRandomDecksFromHand(IHand hand)
    {
        var h = hand.
    }
}

public interface CardInfoDisplayer
{
    void SetActiveCardInfo(string deckTitle, string cardTitle, string cardDescription);
    void SetSelectedCardInfo(string deckTitle, string cardTitle, string cardDescription);
    void Select();
    void Deselect();
}

public class CardInfoUIPanel : MonoBehaviour, CardInfoDisplayer
{
    private string _panelTitle;

    private string _activeCardDeckTitle;
    private string _activeCardTitle;
    private string _activeCardDescription;

    private string activeCard
    private string activeCard


    public void SetActiveCardInfo(string deckTitle, string cardTitle, string cardDescription)
    {
    }

    public void SetSelectedCardInfo(string deckTitle, string cardTitle, string cardDescription)
    {
    }

    public void Select()
    {

    }

    public void Deselect()
    {
    }
}

public class DeckPanel : MonoBehaviour
{
    [SerializeField] private LayoutElement _cardListPort;
    [SerializeField] private LayoutElement _cardListContent;
    [SerializeField] private RectTransform _viewport;

    [SerializeField] private GameObject _cardFramePrefab;

    [SerializeField] private float _padding;
    [SerializeField] private float _spacing;
    [SerializeField] private float _cardWidth;

    [SerializeField] private int _cardCount;

    [Space]
    [Header("Properties")]
    [SerializeField] private TMP_ColorGradient _openedColorGradient;
    [SerializeField] private TMP_ColorGradient _closedColorGradient;
    [SerializeField] private TMP_ColorGradient _obtainedColorGradient;

    private IHand _hand;
    //private LinkedList<GameObject> _cardList;
    //private readonly LinkedList<CardUI> _cardList = new();
    private string _name;

    public void TakeOpenedCard()
    {
        _hand.TakeCardFromDeck(_name);
    }

    public void SetHand(IHand hand)
    {
        _hand = hand;
    }

    private void DisplayDeck(string deckName, int deckSize, int openedCardIndex)
    {
        _name = deckName;
        var containerWidth = deckSize * (_cardWidth + _spacing) + _padding;
        _cardListContent.minWidth = containerWidth;

        var portWidth = containerWidth * 2 - _viewport.rect.width;
        _cardListPort.minWidth = portWidth;
        _cardListPort.preferredWidth = portWidth;
        _cardListPort.flexibleWidth = portWidth;

        for (int i = 0; i < deckSize; i++)
        {
            var cardUIGameObject = Instantiate(_cardFramePrefab, _cardListContent.transform);
            cardUIGameObject.name = $"Card {i}";
            var cardUI = cardUIGameObject.GetComponentInChildren<CardUI>();
            cardUI.SetText($"{i}");
            cardUI.SetColorPresets(_openedColorGradient, _closedColorGradient, _obtainedColorGradient);
            cardUI.SetIndex(i);
            if (i < openedCardIndex)
            {
                cardUI.Obtain();
            }
            if (i == openedCardIndex)
            {
                cardUI.Open();
            }
            if (i > openedCardIndex)
            {
                cardUI.Close();
            }
        }
    }
}