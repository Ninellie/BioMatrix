using Assets.Scripts.Core.Variables.References;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float multiplier;
    [SerializeField] private Vector2Reference _targetPosition;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void FixedUpdate()
    {
        var t = Time.fixedDeltaTime;

        var x = Mathf.Lerp(_transform.position.x, _targetPosition.Value.x, t * multiplier);
        var y = Mathf.Lerp(_transform.position.y, _targetPosition.Value.y, t * multiplier);

        _transform.SetPositionAndRotation(new Vector3(x, y), Quaternion.identity);
    }
}