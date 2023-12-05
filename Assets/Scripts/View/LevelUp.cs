//using System.Collections;
//using System.Collections.Generic;
//using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
//using Assets.Scripts.GameSession.UIScripts.SessionModel;
//using Assets.Scripts.GameSession.Upgrades.Deck;
//using UnityEngine;
//using Button = UnityEngine.UI.Button;
//using Image = UnityEngine.UI.Image;

namespace Assets.Scripts.View
{
    //public class LevelUp : MonoBehaviour
    //{
    //    [SerializeField] private GameObject[] _cardsView;
    //    [SerializeField] private DeckPanel[] _deckView;
    //    [SerializeField] private TMPro.TMP_Text[] _cardsNameText;
    //    [SerializeField] private TMPro.TMP_Text[] _cardsDescriptionText;
    //    [SerializeField] private GameSessionController _gameSessionController;
    //    [SerializeField] private readonly int _givenCardsCount = 3;
    //    [SerializeField] private float _delayBeforeClickableCardsInSeconds;
    //    [SerializeField] private float _pixelsPerStep;
    //    [SerializeField] private float _cardsImageSize;

    //    [SerializeField]
    //    [Range(0, 1)]
    //    private float _initialAlpha;
        
    //    [SerializeField]
    //    [Range(0, 1)]
    //    private float _finalAlpha;

    //    [SerializeField]
    //    private List<Card> _selectedCards = new();

    //    [SerializeField]
    //    private DeckRepository _deckRepository;

    //    [SerializeField]
    //    private EffectsRepository _effectsRepository;

    //    public OverUnitDataAggregator EffectsAggregator { get; set; }

    //    [ContextMenu("DisplayDecks")]
    //    public void DisplayDecks()
    //    {

    //    }

        //[ContextMenu("DisplayCards")]
        //public void DisplayCards()
        //{
        //    foreach (var tmpText in _cardsNameText)
        //    {
        //        tmpText.text = string.Empty;
        //    }

        //    foreach (var tmpText in _cardsDescriptionText)
        //    {
        //        tmpText.text = string.Empty;
        //    }

        //    foreach (var card in _cardsView)
        //    {
        //        card.SetActive(false);
        //    }

        //    _selectedCards = _deckRepository.GetRandomOpenedCards(_givenCardsCount);

        //    if (_selectedCards.Count < 1 || _selectedCards == null)
        //    {
        //        _gameSessionController.Resume();
        //        return;
        //    }

        //    for (var i = 0; i < _selectedCards.Count; i++)
        //    {
        //        _cardsNameText[i].text = _selectedCards[i].title;
        //        _cardsDescriptionText[i].text = _selectedCards[i].description;
        //        StartCoroutine(ActivateCard(_cardsView[i]));
        //    }
        //}

        //public void Improve(int index)
        //{
        //    if (index < 0)
        //    {
        //        return;
        //    }

        //    if (_selectedCards == null)
        //    {
        //        return;
        //    }

        //    if (_selectedCards.Count <= index)
        //    {
        //        return;
        //    }

        //    if (_selectedCards[index] == null)
        //    {
        //        return;
        //    }

            //TakeCard(_selectedCards[index]);
        //}

        //private void TakeCard(Card card)
        //{
        //    _deckRepository.ObtainCard(card);
        //    if (card.effectNames is null) 
        //        return;

        //    foreach (var effectName in card.effectNames)
        //    {
        //        var e = _effectsRepository.GetEffectByName(effectName);
        //        if (e != null)
        //        {
        //            EffectsAggregator.AddEffect(e);
        //        }
        //        else
        //        {
        //            Debug.LogWarning($"Effect with name: {effectName} does not found in repository");
        //        }
        //    }
        //}

    //    private IEnumerator ActivateCard(GameObject card)
    //    {
    //        var cardButton = card.GetComponent<Button>();
    //        var transitionController = card.GetComponent<SelectableTransitionController>();
    //        cardButton.interactable = false;
            
    //        var cardImage = card.GetComponent<Image>();
    //        cardImage.fillAmount = 0;
    //        var cardImageAlpha = 0f;
    //        cardImage.color = new Color(1, 1, 1, cardImageAlpha);

    //        var stepCount = _cardsImageSize / _pixelsPerStep;

    //        var fillingStep = 1 / stepCount;
            
    //        var alphaStep = _finalAlpha / stepCount;

    //        card.SetActive(true);

    //        while (cardImage.fillAmount < 1)
    //        {
    //            var timeBetweenSteps = _delayBeforeClickableCardsInSeconds / stepCount;
    //            yield return new WaitForSecondsRealtime(timeBetweenSteps);
    //            cardImage.fillAmount += fillingStep;
    //            cardImageAlpha += alphaStep;
    //            cardImage.color = new Color(1, 1, 1, cardImageAlpha);
    //        }

    //        cardButton.interactable = true;
    //        transitionController.UpdateColor();
    //    }
    //}
}