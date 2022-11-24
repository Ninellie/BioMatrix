//using UnityEngine;
//using UnityEngine.InputSystem;
//using System;

//public class Player : MonoBehaviour
//{
//    [Header("Player characteristics")]
//    private float currentLifePoints = 10;
//    private float speed;
//    private int _level = 0;
//    private int _experience = 0;
//    private int _expToLvlup = 0;

//    private Vector2 moveVec;
//    private SpriteRenderer sprite;
//    private Movement _movement;

//    public static Action OnGamePaused;
//    public static Action OnCharacterDeath;
//    public static Action OnLevelUp;

//    public bool isFireButtonPressed;


//    public void OnFire()
//    {
//        isFireButtonPressed = true;
//    }
//    public void OnFireOff()
//    {
//        isFireButtonPressed = false;
//    }
//    public void OnMove(InputValue input)
//    {
//        Vector2 inputVec = input.Get<Vector2>();
//        moveVec = new Vector2(inputVec.x, inputVec.y);
//        switch (inputVec.x)
//        {
//            case < 0:
//                sprite.flipX = true;
//                break;
//            case > 0:
//                sprite.flipX = false;
//                break;
//        }
//    }
//    public void OnPause(InputValue input)
//    {
//        OnGamePaused?.Invoke();
//        Debug.Log("Game on pause");
//    }
//    public void OnUnpause(InputValue input)
//    {
//        OnGamePaused?.Invoke();
//        Debug.Log("Game is active");
//    }
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        float newLifePointsValue = currentLifePoints - 2;

//        Debug.Log("Dont touch the Hero!");
//        GameObject otherGO = collision.gameObject;
//        switch (otherGO.tag)
//        {
//            case "Enemy":
//                currentLifePoints = newLifePointsValue;
//                break;
//            case "Projectile":
//                currentLifePoints--;
//                break;
//            case "Boon":
//                _experience++;
//                break;
//        }
//        if (currentLifePoints <= 0)
//        {
//            OnCharacterDeath?.Invoke();
//        }
//    }
//    private void Awake()
//    {
//        _movement = new Movement(gameObject, MovementMode.Rectilinear, speed);
//    }
//    private void Start()
//    {
//        Time.timeScale = 1f;
//        isFireButtonPressed = false;
//        sprite = this.GetComponent<SpriteRenderer>();
//    }
//    private void Update()
//    {
//        if(_level == 0)
//        {
//            _expToLvlup = 10 - _experience;
//        }
//        else
//        {
//            _expToLvlup = 10 + (_level * 2) - _experience;
//        }
        
//        if(_experience >= 10 + (_level * 2))
//        {
//            _experience = 0;
//            _level++;
//            OnLevelUp?.Invoke();
//        }
//    }
//    private void FixedUpdate()
//    {
//        _movement.SetMovementDirection(moveVec);
//    }
//}