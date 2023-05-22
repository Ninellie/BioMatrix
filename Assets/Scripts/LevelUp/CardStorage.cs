using System.Collections.Generic;
using UnityEngine;

public class CardStorage : MonoBehaviour
{
    private readonly List<Card> _cards = new();
    [SerializeField] private Player _player;

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
            if (cardEffect.TargetName == nameof(Player))
            {
                Debug.LogWarning($"Add effect to player {cardEffect.Name}");
                _player.AddEffectStack(cardEffect);
            }
        }
    }

    public void RemoveCard(Card card)
    {
        _cards.Remove(card);
        foreach (var cardEffect in card.Effects)
        {
            _player.RemoveEffectStack(cardEffect);
        }
    }
}