using System.Collections.Generic;
using UnityEngine;

public class CardStorage : MonoBehaviour
{
    private readonly List<Card> _cards = new();
    private Player _player;
    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }
    public void AddCard(Card card)
    {
        _cards.Add(card);
        if (card.Effects is null)
        {
            return;
        }
        foreach (var cardEffect in card.Effects)
        {
            _player.AddEffect(cardEffect);
        }
    }

    public void RemoveCard(Card card)
    {
        _cards.Remove(card);
        foreach (var cardEffect in card.Effects)
        {
            _player.RemoveEffect(cardEffect);
        }
    }
}