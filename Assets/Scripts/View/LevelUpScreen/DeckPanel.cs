using Assets.Scripts.GameSession.Upgrades.Deck;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public class DeckPanel : MonoBehaviour
{
    [SerializeField] private GameObject _cardFramePrefab;
    [SerializeField] private Transform _cardListContainer;

    [SerializeField] private RectTransform _deckContentPanel;
    [SerializeField] private int _cardCount;
    [SerializeField] private float padding;

    private LinkedList<GameObject> _cardList;

    private void Awake()
    {
        //var initialPos = _deckContentPanel.transform.position;
        //var right = _deckContentPanel.right;
        //var containerWidth = _cardCount * (padding + 100) + padding;
        //var containerRightPosition = new Vector2(containerWidth / (padding + 100), 1);
        //_deckContentPanel.anchorMax = containerRightPosition;
        //_deckContentPanel.sizeDelta = containerRightPosition;
        //_deckContentPanel.position = initialPos;


        //for (int i = 0; i < _cardCount; i++)
        //{
        //    var card = Instantiate(_cardFramePrefab, _cardListContainer);
        //    var cardPosition = _deckContentPanel.anchorMin;
        //    cardPosition.x += padding;
        //    cardPosition.x += padding * i;
        //    var card3DPos = new Vector3(cardPosition.x, cardPosition.y, 0f);
        //    card.transform.position= card3DPos;
        //    card.name = $"Card {i}";
        //    card.GetComponentInChildren<TMP_Text>().text = $"{i}";
        //}

        for (int i = 0; i < _cardCount; i++)
        {
            var card = Instantiate(_cardFramePrefab, _cardListContainer);
            card.name = $"Card {i}";
            card.GetComponentInChildren<TMP_Text>().text = $"{i}";
        }
    }
}
