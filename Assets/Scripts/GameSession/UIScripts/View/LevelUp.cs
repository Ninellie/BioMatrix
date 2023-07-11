using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.GameSession.UIScripts.SessionModel;
using Assets.Scripts.GameSession.Upgrades.Deck;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Assets.Scripts.GameSession.UIScripts.View
{
    public class LevelUp : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private GameObject[] _cardsView;
        [SerializeField] private TMPro.TMP_Text[] _cardsNameText;
        [SerializeField] private TMPro.TMP_Text[] _cardsDescriptionText;
        [SerializeField] private ViewController _viewController;
        [SerializeField] private readonly int _givenCardsCount = 3;
        [SerializeField] private float _delayBeforeClickableCardsInSeconds;
        [SerializeField] private float _pixelsPerStep;
        [SerializeField] private float _cardsImageSize;

        private List<Card> _selectedCards = new();
        private static readonly ICardRepository CardRepository = new ArrayCardRepository();
        private readonly CardManager _cardManager = new(CardRepository);

        private void Start()
        {
            _player = FindObjectOfType<Player>();
        }

        [ContextMenu("DisplayCards")]
        public void DisplayCards()
        {
            foreach (var tmpText in _cardsNameText)
            {
                tmpText.text = string.Empty;
            }

            foreach (var tmpText in _cardsDescriptionText)
            {
                tmpText.text = string.Empty;
            }

            foreach (var card in _cardsView)
            {
                card.SetActive(false);
            }

            _selectedCards = _cardManager.GetRandomOpenedCards(_givenCardsCount);

            if (_selectedCards.Count < 1 || _selectedCards == null)
            {
                _viewController.Resume();
                return;
            }

            for (var i = 0; i < _selectedCards.Count; i++)
            {
                _cardsNameText[i].text = _selectedCards[i].Title;
                _cardsDescriptionText[i].text = _selectedCards[i].Description;
                StartCoroutine(ActivateCard(_cardsView[i]));
            }
        }

        public void Improve(int index)
        {
            if (index < 0)
            {
                return;
            }

            if (_selectedCards == null)
            {
                return;
            }

            if (_selectedCards.Count <= index)
            {
                return;
            }

            if (_selectedCards[index] == null)
            {
                return;
            }

            AddCard(_selectedCards[index]);
        }

        public void AddCard(Card card)
        {
            _cardManager.AddCard(card.Id);
            if (card.Effects is null)
            {
                return;
            }
            foreach (var cardEffect in card.Effects)
            {
                if (cardEffect.TargetName != nameof(Player)) continue;

                Debug.LogWarning($"Add effect to player {cardEffect.Name}");
                _player.AddEffectStack(cardEffect);
            }
        }

        public void RemoveCard(Card card)
        {
            foreach (var cardEffect in card.Effects)
            {
                _player.RemoveEffectStack(cardEffect);
            }
        }

        private IEnumerator ActivateCard(GameObject card)
        {
            var cardButton = card.GetComponent<Button>();
            cardButton.interactable = false;
            
            var cardImage = card.GetComponent<Image>();
            cardImage.fillAmount = 0;
            var cardImageAlpha = 0f;
            cardImage.color = new Color(1, 1, 1, cardImageAlpha);
            var fillingStep = 1 / _cardsImageSize * _pixelsPerStep; ;

            card.SetActive(true);

            while (cardImage.fillAmount < 1)
            {
                var fillingAndAlphaStepPerSecond = _delayBeforeClickableCardsInSeconds * Time.unscaledDeltaTime;
                yield return new WaitForSecondsRealtime(fillingAndAlphaStepPerSecond);
                cardImage.fillAmount += fillingStep;
                cardImageAlpha += fillingStep;
                cardImage.color = new Color(1, 1, 1, cardImageAlpha);
            }

            cardButton.interactable = true;
        }
    }
}