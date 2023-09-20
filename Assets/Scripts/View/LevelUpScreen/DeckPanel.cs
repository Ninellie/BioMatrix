using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DeckPanel : MonoBehaviour
{
    [Header("Card panel properties")]
    [SerializeField] private GameObject _cardFramePrefab;
    [Space]
    [SerializeField] private RectTransform _cardListScrollView;
    [SerializeField] private RectTransform _viewport;
    [SerializeField] private RectTransform _cardListContent;
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
    [SerializeField] private bool _isActive;
    
    private readonly LinkedList<CardUI> _cardList = new();
    private LinkedListNode<CardUI> _selectedCard;
    private CardUI _openedCard;


    public void UpdateContentPosition()
    {
        var scrollRect = _cardListScrollView; // This is scroll rect
        var contentRect = _cardListContent; // This is content rect
        var padding = _cardListContent.GetComponent<HorizontalOrVerticalLayoutGroup>().padding;
        var selectedCardRectTransform = _selectedCard.Value.GetComponent<RectTransform>();
        var overflow = (contentRect.rect.width - scrollRect.rect.width) / 2f;
        var leftBorder = overflow - selectedCardRectTransform.offsetMin.x + padding.left;
        var rightBorder = contentRect.rect.width - overflow - selectedCardRectTransform.offsetMax.x - padding.right;

        if (leftBorder > contentRect.anchoredPosition.x)
            contentRect.anchoredPosition = new Vector2(leftBorder, contentRect.anchoredPosition.y);
        else if (rightBorder < contentRect.anchoredPosition.x)
            contentRect.anchoredPosition = new Vector2(rightBorder, contentRect.anchoredPosition.y);
    }
    public void UpdateContentPositionToOpenedCard()
    {
        _cardsScrollRect.horizontalNormalizedPosition = 0f;
        var scrollRect = _cardListScrollView;
        var contentRect = _cardListContent;
        var padding = _cardListContent.GetComponent<HorizontalOrVerticalLayoutGroup>().padding;
        var selectedCardRectTransform = _openedCard.GetComponent<RectTransform>();
        var overflow = (contentRect.rect.width - scrollRect.rect.width) / 2f;
        var leftBorder = overflow - selectedCardRectTransform.offsetMin.x + padding.left;
        var rightBorder = contentRect.rect.width - overflow - selectedCardRectTransform.offsetMax.x - padding.right;

        if (leftBorder > contentRect.anchoredPosition.x)
            contentRect.anchoredPosition = new Vector2(leftBorder, contentRect.anchoredPosition.y);
        else if (rightBorder < contentRect.anchoredPosition.x)
            contentRect.anchoredPosition = new Vector2(rightBorder, contentRect.anchoredPosition.y);
    }

    public void SelectNextCard()
    {
        if (_selectedCard.Next is null)
        {
            Debug.LogWarning($"Next card does not exist");
            return;
        }

        //if (_selectedCard.Previous is not null) _selectedCard.Previous.Value.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        _selectedCard.Next.Value.Select();
        _selectedCard.Value.Deselect();
        _selectedCard = _selectedCard.Next;

        //UpdateCardsScale();

        UpdateContentPosition();
    }

    //private void UpdateCardsScale()
    //{
    //    var selectedScale = new Vector3(1f, 1f, 1);
    //    var neighbourScale = new Vector3(0.75f, 0.75f, 1);

    //    if (!_isActive)
    //    {
    //        neighbourScale = new Vector3(0.5f, 0.5f, 1);
    //        selectedScale = new Vector3(0.5f, 0.5f, 1);
    //    }

    //    if (_selectedCard.Next is not null) _selectedCard.Next.Value.transform.localScale = neighbourScale;
    //    _selectedCard.Value.transform.localScale = selectedScale;
    //    if (_selectedCard.Previous is not null) _selectedCard.Previous.Value.transform.localScale = neighbourScale;
    //}

    public void SelectPreviousCard()
    {
        if (_selectedCard.Previous is null)
        {
            Debug.LogWarning($"Previous card does not exist");
            return;
        }
        //if (_selectedCard.Next is not null) _selectedCard.Next.Value.transform.localScale = new Vector3(0.5f, 0.5f, 1);

        _selectedCard.Previous.Value.Select();
        _selectedCard.Value.Deselect();
        _selectedCard = _selectedCard.Previous;

        //UpdateCardsScale();

        UpdateContentPosition();
    }

    public int GetSelectedCardIndex()
    {
        return _selectedCard.Value.GetIndex();
    }

    public void Activate()
    {
        _isActive = true;
        transform.localScale = new Vector3(1, 1, 1);
        SelectOpenedCard();
        _flapPanel.SetActive(false);
        //UpdateCardsScale();
        UpdateContentPosition();
        //transform.localScale = new Vector3(1.5f, 1.5f, 1);
    }

    public void Deactivate()
    {
        _isActive = false;
        transform.localScale = new Vector3(0.75f, 0.75f, 1);
        _selectedCard.Value.Deselect();
        _flapPanel.SetActive(true);
        //UpdateCardsScale();
        UpdateContentPositionToOpenedCard();
        //transform.localScale = new Vector3(1, 1, 1);
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
        transform.localScale = new Vector3(0.75f, 0.75f, 1);
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
            //cardUI.transform.localScale = new Vector3(0.5f, 0.5f, 1);

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