using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DeckPanel : MonoBehaviour
{
    [Header("Card panel properties")]
    [SerializeField] private GameObject _cardFramePrefab;
    [Space]
    [SerializeField] private LayoutElement _cardListPort;
    [SerializeField] private LayoutElement _cardListContent;
    [SerializeField] private RectTransform _viewport;
    [Space]
    [SerializeField] private float _padding;
    [SerializeField] private float _spacing;
    [SerializeField] private float _cardWidth;
    [Space]
    [SerializeField] private GameObject _flapPanel;
    [SerializeField] private ScrollRect _cardsScrollRect;

    [Space]
    [Header("Deck naming properties")]
    [SerializeField] private TMP_Text _deckNameText;
    //[SerializeField] private Image _deckIcon; TODO icon info taken from deck repository from scriptableObject prefab

    [Space]
    [Header("Color presets")]
    [SerializeField] private TMP_ColorGradient _openedColorGradient;
    [SerializeField] private TMP_ColorGradient _closedColorGradient;
    [SerializeField] private TMP_ColorGradient _obtainedColorGradient;

    [Space]
    [Header("Properties")]
    [SerializeField] private string _name;
    
    private readonly LinkedList<CardUI> _cardList = new();
    private LinkedListNode<CardUI> _selectedCard;
    private CardUI _openedCard;

    public int GetSelectedCardIndex()
    {
        return _selectedCard.Value.GetIndex();
    }

    public void SelectNextCard()
    {
        if (_selectedCard.Next is null)
        {
            Debug.LogWarning($"Previous card does not exist");
            return;
        }

        _selectedCard.Next.Value.Select();
        _selectedCard.Value.Deselect();
        _selectedCard = _selectedCard.Next;
    }

    public void SelectPreviousCard()
    {
        if (_selectedCard.Previous is null)
        {
            Debug.LogWarning($"Previous card does not exist");
            return;
        }

        _selectedCard.Previous.Value.Select();
        _selectedCard.Value.Deselect();
        _selectedCard = _selectedCard.Previous;
    }

    public void Activate()
    {
        SelectOpenedCard();
        _flapPanel.SetActive(false);
        var selectedCardIndex = _selectedCard.Value.GetIndex();
        var cardsCount = _cardList.Count;
        var step = 1f / cardsCount;
        var selectedCardNormalizedPosition = step * selectedCardIndex;
        _cardsScrollRect.verticalNormalizedPosition = selectedCardNormalizedPosition;

    }

    public void Deactivate()
    {
        // Deselect all cards
        _flapPanel.SetActive(false);
        _cardsScrollRect.verticalNormalizedPosition = 0;
    }

    public string GetName()
    {
        return _name;
    }

    private void SelectOpenedCard()
    {
        _selectedCard = _cardList.Find(_openedCard);
        _selectedCard.Value.Select();
    }

    private void DeselectSelectedCard()
    {
        _selectedCard.Value.Deselect();
    }

    public void DisplayDeck(string deckName, int deckSize, int openedCardIndex)
    {
        _cardList.Clear();
        _name = deckName;
        _deckNameText.text = _name;
        var containerWidth = deckSize * (_cardWidth + _spacing) + _padding;
        _cardListContent.minWidth = containerWidth;

        var portWidth = containerWidth * 2 - _viewport.rect.width;
        _cardListPort.minWidth = portWidth;
        _cardListPort.preferredWidth = portWidth;
        _cardListPort.flexibleWidth = portWidth;

        for (int i = 0; i < deckSize; i++)
        {
            var cardUIGameObject = Instantiate(_cardFramePrefab, _cardListContent.transform);
            cardUIGameObject.name = $"{deckName} Card {i}";
            var cardUI = cardUIGameObject.GetComponentInChildren<CardUI>();
            var cardNode = _cardList.AddLast(cardUI);
            cardUI.SetText($"{i}");
            cardUI.SetColorTextPresets(_openedColorGradient, _closedColorGradient, _obtainedColorGradient);
            cardUI.SetIndex(i);

            if (i < openedCardIndex)
            {
                cardUI.Obtain();
            }
            if (i == openedCardIndex)
            {
                cardUI.Open();
                _openedCard = cardUI;
                _selectedCard = cardNode;
            }
            if (i > openedCardIndex)
            {
                cardUI.Close();
            }
        }
    }
}