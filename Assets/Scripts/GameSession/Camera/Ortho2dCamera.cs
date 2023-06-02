using UnityEngine;

namespace Assets.Scripts.GameSession.Camera
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(UnityEngine.Camera))]
    internal class Ortho2dCamera : MonoBehaviour
    {
        [SerializeField] private bool _uniform = true;
        [SerializeField] private bool _autoSetUniform = false;

        private void Awake()
        {
            GetComponent<UnityEngine.Camera>().orthographic = true;
            if (_uniform)
                SetUniform();
        }
        private void LateUpdate()
        {
            if (_autoSetUniform && _uniform)
                SetUniform();
        }
        private void SetUniform()
        {
            var orthographicSize = GetComponent<UnityEngine.Camera>().pixelHeight / 2;
            if (orthographicSize != GetComponent<UnityEngine.Camera>().orthographicSize)
                GetComponent<UnityEngine.Camera>().orthographicSize = orthographicSize;
        }
    }
}