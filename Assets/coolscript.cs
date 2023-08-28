using UnityEngine;
using UnityEngine.Events;

public class coolscript : MonoBehaviour
{
    private UnityEvent myEvent = new();

    void Start()
    {
        
    }

    void OnEnable()
    {
        myEvent.AddListener(MyAction);
    }
    void OnDisable()
    {
        myEvent.RemoveListener(MyAction);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myEvent.Invoke();
    }

    private void MyAction()
    {
        Debug.LogWarning("Action");
    }
}
