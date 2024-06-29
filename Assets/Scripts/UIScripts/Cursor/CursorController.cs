using UnityEngine;

namespace UIScripts.Cursor
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private Texture2D aimCursor;
        [SerializeField] private Texture2D reloadCursor;
        
        private void Awake()
        {
            var controllers = FindObjectsOfType<CursorController>();
            foreach (var cursorController in controllers)
            {
                if (cursorController != this)
                {
                    Destroy(cursorController);
                }
            }
            
            SetAimCursor();
            DontDestroyOnLoad(this);
        }

        public void SetAimCursor()
        {
            UnityEngine.Cursor.SetCursor(aimCursor, new Vector2(16, 48), CursorMode.Auto);
        }
        
        public void SetReloadCursor()
        {
            UnityEngine.Cursor.SetCursor(reloadCursor, new Vector2(16, 48), CursorMode.Auto);
        }
    }
}
