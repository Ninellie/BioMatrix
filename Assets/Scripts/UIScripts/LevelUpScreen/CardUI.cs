using GameSession.Upgrades.Deck;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIScripts.LevelUpScreen
{
    public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Components references")]
        [SerializeField] private Image _blackoutFlap;
        [SerializeField] private Image _frame;
        [SerializeField] private Image _selectedFrame;
        [SerializeField] private Image _magicFrame;
        [SerializeField] private Image _rareFrame;
        [SerializeField] private TMP_Text _text;
        [Space]
        [Header("Properties")]
        [SerializeField] private TMP_ColorGradient _openedColorGradient;
        [SerializeField] private TMP_ColorGradient _closedColorGradient;
        [SerializeField] private TMP_ColorGradient _obtainedColorGradient;
        [Space]
        [Header("Don't change!")]
        [SerializeField] private CardStatus _status;
        [SerializeField] private string _deckName;
        [SerializeField] private int _index;
        [SerializeField] private bool _isOpened;

        private CardInfoUIPanel _cardInfoPanel;
        private DeckPanel _deckPanel;

        public void SetDeckPanel(DeckPanel deckPanel) => _deckPanel = deckPanel;
        public void SetCardInfoPanel(CardInfoUIPanel cardInfoPanel) => _cardInfoPanel = cardInfoPanel;
        public void SetDeckName(string deckName) => _deckName = deckName;
        public void SetIndex(int index) => _index = index;
        //public int GetIndex() => _index;

        private void SetFrameSelected(bool outline)
        {
            _selectedFrame.gameObject.SetActive(outline);
        }

        public void TurnToNormal()
        {
            _frame.gameObject.SetActive(true);
            _magicFrame.gameObject.SetActive(false);
            _rareFrame.gameObject.SetActive(false);
        }

        public void TurnToMagic()
        {
            _frame.gameObject.SetActive(true);
            _magicFrame.gameObject.SetActive(true);
            _rareFrame.gameObject.SetActive(false);
        }

        public void TurnToRare()
        {
            _frame.gameObject.SetActive(true);
            _magicFrame.gameObject.SetActive(false);
            _rareFrame.gameObject.SetActive(true);
        }

        public void TurnToGreat()
        {
            _frame.gameObject.SetActive(true);
            _magicFrame.gameObject.SetActive(true);
            _rareFrame.gameObject.SetActive(true);
        }

        public void ToggleSelected(bool isSelected)
        {
            if (isSelected)
                Select();
            else
                Deselect();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var openedCard = _deckPanel.GetOpenedCard();
            if (Equals(openedCard)) return;
            Select();
            openedCard.Deselect();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            var openedCard = _deckPanel.GetOpenedCard();
            if (Equals(openedCard)) return;
            Deselect();
            if (!_deckPanel.IsActive) return;
            openedCard.Select();
        }

        public void Select()
        {
            _selectedFrame.gameObject.SetActive(true);
            if (!_deckPanel.IsActive) return;
            _cardInfoPanel.DisplayCardInfo(_deckName, _index);
        }

        public void Deselect()
        {
            _selectedFrame.gameObject.SetActive(false);
        }

        public void SetColorTextPresets(TMP_ColorGradient openedColorGradient,
            TMP_ColorGradient closedColorGradient,
            TMP_ColorGradient obtainedColorGradient)
        {
            _openedColorGradient = openedColorGradient;
            _closedColorGradient = closedColorGradient;
            _obtainedColorGradient = obtainedColorGradient;
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        [ContextMenu(nameof(Open))]
        public void Open()
        {
            _status = CardStatus.Opened;
            _blackoutFlap.color = new Color(0, 0, 0, 0f);
            _blackoutFlap.gameObject.SetActive(false);
            _text.colorGradientPreset = _openedColorGradient;

        }

        [ContextMenu(nameof(Obtain))]
        public void Obtain()
        {
            _status = CardStatus.Obtained;
            _blackoutFlap.color = new Color(0, 0, 0, 0f);
            _blackoutFlap.gameObject.SetActive(true);

            _text.colorGradientPreset = _obtainedColorGradient;
        }

        [ContextMenu(nameof(Close))]
        public void Close()
        {
            _status = CardStatus.Closed;
            _blackoutFlap.gameObject.SetActive(true);
            _blackoutFlap.color = new Color(0, 0, 0, 0.2f);
            _text.colorGradientPreset = _closedColorGradient;
        }
    }
}