using Assets.Scripts.GameSession.Upgrades.Deck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [Header("Components references")]
    [SerializeField] private Image _blackoutFlap;
    [SerializeField] private Image _frame;
    [SerializeField] private Image _selectedFrame;
    [SerializeField] private Image _magicFrame;
    [SerializeField] private Image _rareFrame;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _background;
    [SerializeField] private LayoutElement _layoutElement;
    [Space]
    [Header("Properties")]
    [SerializeField] private TMP_ColorGradient _openedColorGradient;
    [SerializeField] private TMP_ColorGradient _closedColorGradient;
    [SerializeField] private TMP_ColorGradient _obtainedColorGradient;
    [Space]
    [Header("Don't change!")]
    [SerializeField] private CardStatus _status;
    [SerializeField] private bool _isSelected;
    [SerializeField] private int _index;


    private DeckPanel _deckPanel;

    public void SetIndex(int index) => _index = index;

    public int GetIndex() => _index;

    private void SetFrameSelected(bool outline)
    {
        _selectedFrame.gameObject.SetActive(outline);
        _frame.gameObject.SetActive(!outline);
    }

    public void TurnToNormal()
    {
        _frame.gameObject.SetActive(true);
        _magicFrame.gameObject.SetActive(false);
        _rareFrame.gameObject.SetActive(false);
        //transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }

    public void TurnToMagic()
    {
        _frame.gameObject.SetActive(true);
        _magicFrame.gameObject.SetActive(true);
        _rareFrame.gameObject.SetActive(false);
        //transform.localScale = new Vector3(0.75f, 0.75f, 1);
    }

    public void TurnToRare()
    {
        _frame.gameObject.SetActive(true);
        _magicFrame.gameObject.SetActive(true);
        _rareFrame.gameObject.SetActive(true);
        //transform.localScale = new Vector3(1, 1, 1);
    }

    public void Select()
    {
        //transform.localScale = new Vector3(1.5f, 1.5f, 1);
        SetFrameSelected(true);
        _isSelected = true;
    }

    public void Deselect()
    {
        SetFrameSelected(false);
        _isSelected = false;
        //transform.localScale = new Vector3(1, 1, 1);
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