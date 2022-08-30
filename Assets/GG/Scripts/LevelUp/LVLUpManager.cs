using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLUpManager : MonoBehaviour
{
    public GameObject timer;
    private ArrayCardRepository cardRepository;
    private CardManager cardManager;

    private void CardsFill(Card[] cards)
    {
        
        for (int i = 0; i> cardRepository.CardCount; i++)
        {
            cards[i] = cardRepository.Get(i);
        }
    }

    private void CardsDisplay()
    {
        Card[] cards = new Card[cardRepository.CardCount];

        CardsFill(cards);

        Card[] SelectedCards = new Card[3];
    }
}
