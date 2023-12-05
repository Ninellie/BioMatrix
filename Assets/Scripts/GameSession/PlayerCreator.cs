using Assets.Scripts.GameSession.Upgrades.Deck;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class PlayerCreator : MonoBehaviour
    {
        [SerializeField] private LevelUpController _levelUpController;

        private EffectsRepository _effectsRepository;
        private IDeckRepository _deckRepository;
        private IHand _hand;

        private void Awake()
        {
            _effectsRepository = FindObjectOfType<EffectsRepository>();
            _deckRepository = FindObjectOfType<PatternDeckRepository>();
        }

        private void CreatePlayer(GameObject playerPrefab)
        {
            //_hand = player.GetComponent<IHand>();
            _hand.SetDeckRepository(_deckRepository);
            _hand.SetEffectRepository(_effectsRepository);
            _levelUpController.SetHand(_hand);
        }
    }
}