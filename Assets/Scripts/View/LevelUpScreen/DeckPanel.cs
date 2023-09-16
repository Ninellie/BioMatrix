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
    //private readonly LinkedList<CardUI> _cardList = new();
    private string _name;

    public string GetName()
    {
        return _name;
    }

    public void DisplayDeck(string deckName, int deckSize, int openedCardIndex)
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