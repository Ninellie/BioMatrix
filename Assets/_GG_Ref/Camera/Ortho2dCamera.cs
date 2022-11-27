using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
internal class Ortho2dCamera : MonoBehaviour
{
    [SerializeField] private bool _uniform = true;
    [SerializeField] private bool _autoSetUniform = false;

    private void Awake()
    {
        GetComponent<Camera>().orthographic = true;
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
        var orthographicSize = GetComponent<Camera>().pixelHeight / 2;
        if (orthographicSize != GetComponent<Camera>().orthographicSize)
            GetComponent<Camera>().orthographicSize = orthographicSize;
    }
}