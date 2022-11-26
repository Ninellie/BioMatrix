using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;

    private float _disappearTimer;
    private readonly float _moveYSpeed = 20f;
    private readonly float _moveXSpeed = 10f;
    private Color _textColor;
    private void Awake()
    {
        textMeshPro = gameObject.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        
        transform.position += new Vector3(_moveXSpeed, _moveYSpeed) * Time.deltaTime;

        _disappearTimer -= Time.deltaTime;
        if(_disappearTimer < 0)
        {
            //Start disapearing
            var disapearingSpeed = 3f;
            _textColor.a -= disapearingSpeed * Time.deltaTime;
            textMeshPro.color = _textColor;
            if (_textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Setup(int damageAmount)
    {
        textMeshPro.SetText(damageAmount.ToString());
        _textColor = textMeshPro.color;
        _disappearTimer = 1f;
    }
}
