using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CardState
{
    Opened,
    Closed,
    Obtained
}

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
    [SerializeField] private CardState _state;
    [SerializeField] private bool _isSelected;
    
    [SerializeField] private string _description;

    
    public void SetColorPresets(TMP_ColorGradient openedColorGradient, TMP_ColorGradient closedColorGradient,
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

    public void SetDescription(string text)
    {
        _description = text;
    }

    [ContextMenu(nameof(Open))]
    public void Open()
    {
        _state = CardState.Opened;
        _blackoutFlap.color = new Color(0, 0, 0, 0f);
        _blackoutFlap.gameObject.SetActive(false);
        _text.colorGradientPreset = _openedColorGradient;

    }

    [ContextMenu(nameof(Obtain))]
    public void Obtain()
    {
        _state = CardState.Obtained;
        _blackoutFlap.color = new Color(0, 0, 0, 0f);
        _blackoutFlap.gameObject.SetActive(true);

        _text.colorGradientPreset = _obtainedColorGradient;
    }

    [ContextMenu(nameof(Close))]
    public void Close()
    {
        _state = CardState.Closed;
        _blackoutFlap.color = new Color(0, 0, 0, 0);
        _blackoutFlap.gameObject.SetActive(false);
        _text.colorGradientPreset = _closedColorGradient;
    }
}