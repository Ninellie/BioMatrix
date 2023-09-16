using Assets.Scripts.GameSession.Upgrades.Deck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [Header("Components references")]
    [SerializeField] private Image _blackoutFlap;
    [SerializeField] private Image _frame;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _background;
    [Space]
    [Header("Properties")]
    [SerializeField] private TMP_ColorGradient _openedColorGradient;
    [SerializeField] private TMP_ColorGradient _closedColorGradient;
    [SerializeField] private TMP_ColorGradient _obtainedColorGradient;
    [Space]
    [SerializeField] private CardStatus _status;
    [SerializeField] private bool _isSelected;
    [SerializeField] private int _index;

    private DeckPanel _deckPanel;

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void Take()
    { 
        //_deckPanel.TakeOpenedCard();
    }

    public void SetColorPresets(TMP_ColorGradient openedColorGradient, 
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
        _blackoutFlap.color = new Color(0, 0, 0, 0);
        _blackoutFlap.gameObject.SetActive(false);
        _text.colorGradientPreset = _closedColorGradient;
    }
}