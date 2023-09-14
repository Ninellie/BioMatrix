using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    //private LinkedList<GameObject> _cardList;
    private readonly LinkedList<CardUI> _cardList = new();

    private void Awake()
    {
        var containerWidth = _cardCount * (_cardWidth + _spacing) + _padding;
        _cardListContent.minWidth = containerWidth;

        var portWidth = containerWidth * 2 - _viewport.rect.width;
        _cardListPort.minWidth = portWidth;
        _cardListPort.preferredWidth = portWidth;
        _cardListPort.flexibleWidth = portWidth;

        int openedCardNumber = Random.Range(1, _cardCount + 1);
        for (int i = 1; i <= _cardCount; i++)
        {
            var card = Instantiate(_cardFramePrefab, _cardListContent.transform);
            card.name = $"Card {i}";
            var cardUI = card.GetComponentInChildren<CardUI>();
            _cardList.AddLast(cardUI);
            cardUI.SetText($"{i}");
            cardUI.SetColorPresets(_openedColorGradient, _closedColorGradient, _obtainedColorGradient);

            if (i < openedCardNumber)
            {
                cardUI.Obtain();
            }
            if (i == openedCardNumber)
            {
                cardUI.Open();
            }
            if (i > openedCardNumber)
            {
                cardUI.Close();
            }
        }
    }

    public void SetDeck()
    {

    }

    public void FillDeck()
    {

    }
}
