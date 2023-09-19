using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DeckPanel : MonoBehaviour
{
    [Header("Card panel properties")]
    [SerializeField] private GameObject _cardFramePrefab;
    [Space]
    //[SerializeField] private LayoutElement _cardListPort;
    //[SerializeField] private LayoutElement _cardListContent;
    [SerializeField] private RectTransform _cardListScrollView; // Scrollview
    [SerializeField] private RectTransform _viewport; // Viewport
    [SerializeField] private RectTransform _cardListContent; // This is Content
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


    public void UpdateContentPosition()
    {
        var portRect = _cardListScrollView; // This is scroll rect
        var contentRect = _cardListContent; // This is content rect
        var selectedCardRectTransform = _selectedCard.Value.GetComponent<RectTransform>();
        var overflow = (contentRect.rect.width - portRect.rect.width) / 2f;
        var leftBorder = overflow - selectedCardRectTransform.offsetMin.x;
        var rightBorder = contentRect.rect.width - overflow - selectedCardRectTransform.offsetMax.x;

        if (leftBorder > contentRect.anchoredPosition.x)
            contentRect.anchoredPosition = new Vector2(leftBorder + _padding, contentRect.anchoredPosition.y);
        else if (rightBorder < contentRect.anchoredPosition.x)
            contentRect.anchoredPosition = new Vector2(rightBorder - _padding, contentRect.anchoredPosition.y);
    }
    public void UpdateContentPositionToOpenedCard()
    {
        _cardsScrollRect.horizontalNormalizedPosition = 0f;
        var portRect = _cardListScrollView; // This is scroll rect
        var contentRect = _cardListContent; // This is content rect
        var selectedCardRectTransform = _openedCard.GetComponent<RectTransform>();
        var overflow = (contentRect.rect.width - portRect.rect.width) / 2f;
        var leftBorder = overflow - selectedCardRectTransform.offsetMin.x;
        var rightBorder = contentRect.rect.width - overflow - selectedCardRectTransform.offsetMax.x;

        if (leftBorder > contentRect.anchoredPosition.x)
            contentRect.anchoredPosition = new Vector2(leftBorder + _padding, contentRect.anchoredPosition.y);
        else if (rightBorder < contentRect.anchoredPosition.x)
            contentRect.anchoredPosition = new Vector2(rightBorder - _padding, contentRect.anchoredPosition.y);
    }

    public void SelectNextCard()
    {
        if (_selectedCard.Next is null)
        {
            Debug.LogWarning($"Next card does not exist");
            return;
        }
        _selectedCard.Next.Value.Select();
        _selectedCard.Value.Deselect();
        _selectedCard = _selectedCard.Next;
        UpdateContentPosition();
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
        UpdateContentPosition();
    }

    public int GetSelectedCardIndex()
    {
        return _selectedCard.Value.GetIndex();
    }

    public void Activate()
    {
        SelectOpenedCard();
        _flapPanel.SetActive(false);
        UpdateContentPosition();
    }

    public void Deactivate()
    {
        _selectedCard.Value.Deselect();
        _flapPanel.SetActive(true);
        UpdateContentPositionToOpenedCard();
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

    //private void DeselectSelectedCard()
    //{
    //    _selectedCard.Value.Deselect();
    //}

    public void DisplayDeck(string deckName, int deckSize, int openedCardIndex)
    {
        _cardList.Clear();
        _name = deckName;
        _deckNameText.text = _name;

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

            //if ((i + 1) % 15 == 0)
            //{
            //    cardUI.TurnToGreat();
            //    continue;
            //}

            if ((i + 1) % 5 == 0)
            {
                cardUI.TurnToRare();
                continue;
            }

            if ((i + 1) % 3 == 0)
            {
                cardUI.TurnToMagic();
                continue;
            }
            
            cardUI.TurnToNormal();
        }

        UpdateContentPositionToOpenedCard();
    }
}