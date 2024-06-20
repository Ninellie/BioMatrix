using UnityEngine;

namespace UIScripts.Cursor
{
    public class CursorController : MonoBehaviour
    {
        private void Awake()
        {
            
            DontDestroyOnLoad(gameObject);
        }
    }
}
