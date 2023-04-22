using UnityEngine;

public class Shield : MonoBehaviour
{
    private int _maxLayers = 3;
    private float _alphaPerLayer = 0.2f;
    private int _layerCount = 3;
    //private float knockbackPower = 300;
    private CircleCollider2D _circleCollider;
    //[SerializeField] private GameObject _knockbackEffector;
    private SpriteRenderer _spriteRenderer;
    private Color _color;
    private GameObject _player;
    private float _disappearTimer;
    private float _disappearingSpeed = 3f;

    private void FixedUpdate()
    {
        transform.position = _player.transform.position;
    }
    private void Awake()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _color = _spriteRenderer.color;
        _player = GetComponentInParent<Player>().gameObject;
    }
    private void Start()
    {
        _circleCollider.enabled = true;
        //_knockbackEffector.SetActive(false);
        RefreshColor();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionGameObject = collision.gameObject;
        collisionGameObject.GetComponent<Enemy>()._enemyMoveController.KnockBackFromTarget(1500);
        switch (collisionGameObject.tag)
        {
            case "Enemy":
                Debug.Log($"Enemy named: {collisionGameObject.name} collide with shield");
                
                _color.a = 1f;
                RemoveActiveLayer();
                //RefreshColor();
                _spriteRenderer.color = _color;
                //_knockbackEffector.SetActive(true);
                break;
        }
    }
    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    var collisionGameObject = collider.gameObject;
    //    switch (collisionGameObject.tag)
    //    {
    //        case "Enemy":
    //            Debug.Log($"Enemy named: {collisionGameObject.name} collide with shield");
    //            collisionGameObject.GetComponent<Enemy>()._enemyMoveController.KnockBackFromTarget(300);
    //            _color.a = 1f;
    //            RemoveActiveLayer();
    //            //RefreshColor();
    //            _spriteRenderer.color = _color;
    //            //_knockbackEffector.SetActive(true);
    //            break;
    //    }
    //}
    private void Update()
    {
        ReduceTransparent();
    }

    private void ReduceTransparent()
    {
        if (!(_color.a > _alphaPerLayer * _layerCount))
        {
            //_knockbackEffector.SetActive(false);
            return;
        }
        _color.a -= _disappearingSpeed * Time.deltaTime;
        _spriteRenderer.color = _color;
    }
    private void RefreshColor()
    {
        var a = _alphaPerLayer * _layerCount;
        _color.a = a;
        _spriteRenderer.color = _color;
        Debug.LogWarning($"Current shield alpha is: {_color.a}");
    }
    public void AddActiveLayer()
    {
        Debug.Log($"Try to add shield layer");
        Debug.Log($"Current number active shield layers: {_layerCount}");
        if (_layerCount == _maxLayers) return;
        _layerCount++;
        _circleCollider.enabled = true;
        Debug.Log($"Current number active shield layers: {_layerCount}");
    }

    public void RemoveActiveLayer()
    {
        Debug.Log($"Try to remove shield layer");
        Debug.Log($"Current number active shield layers: {_layerCount}");
        if (_layerCount == 0) return;
        _layerCount--;
        if (_layerCount == 0)
        {
            _circleCollider.enabled = false;
        }
        Debug.Log($"Current number active shield layers: {_layerCount}");
    }
}
