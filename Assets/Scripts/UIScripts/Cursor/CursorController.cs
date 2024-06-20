using UnityEngine;

namespace UIScripts.Cursor
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private Texture2D aimCursor;
        [SerializeField] private Texture2D reloadCursor;
        
        private void Awake()
        {
            SetAimCursor();
            DontDestroyOnLoad(gameObject);
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
