using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;

    private float disappearTimer;
    private float moveYSpeed = 20f;
    private float moveXSpeed = 10f;
    private Color textColor;
    private void Awake()
    {
        textMeshPro = gameObject.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        
        transform.position += new Vector3(moveXSpeed, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            //Start disapearing
            float disapearingSpeed = 3f;
            textColor.a -= disapearingSpeed * Time.deltaTime;
            textMeshPro.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Setup(int damageAmount)
    {
        textMeshPro.SetText(damageAmount.ToString());
        textColor = textMeshPro.color;
        disappearTimer = 1f;
    }
}
