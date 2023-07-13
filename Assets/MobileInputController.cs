using UnityEngine;

public class MobileInputController : MonoBehaviour
{
    [SerializeField] private GameObject[] _controllers;
    
    private void Awake()
    {

        gameObject.SetActive(Application.isMobilePlatform);
    }

    public void EnableTouchScreenControllers()
    {
        foreach (var controller in _controllers)
        {
            controller.SetActive(true);
        }
    }
    
    public void DisableTouchScreenControllers()
    {
        foreach (var controller in _controllers)
        {
            controller.SetActive(false);
        }
    }
}
