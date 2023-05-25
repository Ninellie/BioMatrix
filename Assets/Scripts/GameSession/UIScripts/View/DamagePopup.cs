using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMeshPro;

    private float _disappearTimer;
    private readonly float _moveYSpeed = 20f;
    private readonly float _moveXSpeed = 10f;
    private Color _textColor;
    private void Awake()
    {
        _textMeshPro = gameObject.GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        Disappear();
    }

    private void Disappear()
    {
        transform.position += new Vector3(_moveXSpeed, _moveYSpeed) * Time.deltaTime;
        _disappearTimer -= Time.deltaTime;
        if (!(_disappearTimer < 0)) return;
        var disappearingSpeed = 3f;
        _textColor.a -= disappearingSpeed * Time.deltaTime;
        _textMeshPro.color = _textColor;
        if (_textColor.a < 0)
        {
            Destroy(gameObject);
        }
    }
    public void Setup(int damageAmount)
    {
        _textMeshPro.SetText(damageAmount.ToString());
        _textColor = _textMeshPro.color;
        _disappearTimer = 1f;
    }
}