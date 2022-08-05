using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
internal class Ortho2dCamera : MonoBehaviour
{
    [SerializeField] private bool uniform = true;
    [SerializeField] private bool autoSetUniform = false;

    private void Awake()
    {
        GetComponent<Camera>().orthographic = true;

        if (uniform)
            SetUniform();
    }
    private void LateUpdate()
    {
        if (autoSetUniform && uniform)
            SetUniform();
    }
    private void SetUniform()
    {
        float orthographicSize = GetComponent<Camera>().pixelHeight / 2;
        if (orthographicSize != GetComponent<Camera>().orthographicSize)
            GetComponent<Camera>().orthographicSize = orthographicSize;
    }
}
