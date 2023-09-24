using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DeckPanel : MonoBehaviour, 
    IPointerClickHandler, IPointerDownHandler, 
    IPointerUpHandler, IPointerEnterHandler, 
    IPointerExitHandler
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
    
    [SerializeField] private CardInfoUIPanel _cardInfo;

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
    
    [SerializeField] private ComplexLevelUpDisplay _parentDisplay;
    public bool IsActive => _isActive;

    private readonly LinkedList<CardUI> _cardList = new();
    private CardUI _openedCard;
    private bool _isSelected;

    public void SetCardInfo(CardInfoUIPanel cardInfo) => _cardInfo = cardInfo;
    public void SetParentDisplay(ComplexLevelUpDisplay display) => _parentDisplay = display;
    public string GetName() => _name;
    public CardUI GetOpenedCard() => _openedCard;

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

    public void Activate()
    {
        _isActive = true;
        transform.localScale = new Vector3(1, 1, 1);
        _flapPanel.SetActive(false);
        UpdateContentPositionToOpenedCard();
        _openedCard.Select();
        _cardInfo.DisplayOpenedCardInfo(_name);
    }

    public void Deactivate()
    {
        _isActive = false;
        transform.localScale = new Vector3(0.75f, 0.75f, 1);
        _flapPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        _flapPanel.SetActive(true);
        UpdateContentPositionToOpenedCard();
        _openedCard.Deselect();
    }

    public void DisplayDeck(string deckName, int deckSize, int openedCardIndex, ToggleGroup cardsToggleGroup)
    {
        _cardList.Clear();
        transform.localScale = new Vector3(0.75f, 0.75f, 1);
        _name = deckName;
        _deckNameText.text = _name;

        for (int i = 0; i < deckSize; i++)
        {
            var cardUIGameObject = Instantiate(_cardFramePrefab, _cardListContent.transform);
            cardUIGameObject.name = $"{deckName} Card {i + 1}";
            var cardUI = cardUIGameObject.GetComponentInChildren<CardUI>();
            var cardNode = _cardList.AddLast(cardUI);
            cardUI.SetText($"{i + 1}");
            cardUI.SetColorTextPresets(_openedColorGradient, _closedColorGradient, _obtainedColorGradient);
            cardUI.SetIndex(i);
            cardUI.SetDeckName(_name);
            cardUI.SetCardInfoPanel(_cardInfo);
            cardUI.SetDeckPanel(this);

            var toggle = cardUIGameObject.GetComponent<Toggle>();
            toggle.group = cardsToggleGroup;

            if (i < openedCardIndex)
            {
                cardUI.Obtain();
            }
            if (i == openedCardIndex)
            {
                cardUI.Open();
                _openedCard = cardUI;
            }
            if (i > openedCardIndex)
            {
                cardUI.Close();
            }

            if ((i + 1) % 15 == 0)
            {
                cardUI.TurnToGreat();
                continue;
            }

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isActive) return;
        _parentDisplay.ActivateDeck(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isActive) return;
        transform.localScale = new Vector3(1, 1, 1);
        _parentDisplay.GetActiveDeck().transform.localScale = new Vector3(0.75f, 0.75f, 1);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isActive) return;
        transform.localScale = new Vector3(0.75f, 0.75f, 1);
        _parentDisplay.GetActiveDeck().transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isActive) return;
        _flapPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isActive) return;
        _flapPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        transform.localScale = new Vector3(0.75f, 0.75f, 1);
    }
}