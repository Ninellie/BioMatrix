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
    [SerializeField] private int _cardCount;
    [SerializeField] private float _padding;
    [SerializeField] private float _spacing;
    [SerializeField] private float _cardWidth;

    private LinkedList<GameObject> _cardList;

    private void Awake()
    {
        var containerWidth = _cardCount * (_cardWidth + _spacing) + _padding;
        _cardListContent.minWidth = containerWidth;

        var portWidth = containerWidth * 2 - _viewport.rect.width;
        _cardListPort.minWidth = portWidth;
        _cardListPort.preferredWidth = portWidth;
        _cardListPort.flexibleWidth = portWidth;

        for (int i = 0; i < _cardCount; i++)
        {
            var card = Instantiate(_cardFramePrefab, _cardListContent.transform);
            card.name = $"Card {i}";
            card.GetComponentInChildren<TMP_Text>().text = $"{i}";
        }
    }
}
