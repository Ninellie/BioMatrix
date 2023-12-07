using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    [ExecuteInEditMode]
    public class StackPool : MonoBehaviour
    {
        [SerializeField] [Range(0, 50)] private int _size;
        [SerializeField] [Range(0, 50)] private int _active;
        [SerializeField] private GameObject _stack;
        [SerializeField] private Transform _transform;

        private readonly Stack<GameObject> _enabled = new();
        private readonly Stack<GameObject> _disabled = new();

        private void Awake()
        {
            _transform = transform;
            UpdateSize();
        }

        public void Add()
        {
            if (!_disabled.TryPop(out var stack)) return;
            stack.SetActive(true);
            _enabled.Push(stack);
        }

        public void Release()
        {
            if (!_enabled.TryPop(out var stack)) return;
            stack.SetActive(false);
            _disabled.Push(stack);
        }

        public void DisableAll()
        {
            for (int i = 0; i < _enabled.Count; i++)
            {
                var stack = _enabled.Pop();
                stack.SetActive(false);
                _disabled.Push(stack);
            }
            _enabled.Clear();
        }

        private void Reset()
        {
            DisableAll();
            for (int i = 0; i < _disabled.Count; i++)
            {
                Destroy(_disabled.Pop());
            }
            _disabled.Clear();
            UpdateSize();
        }

        private void OnValidate()
        {
            _active = Mathf.Min(_active, _size);
            if (_transform == null) _transform = transform;
            if (_stack == null) return;
            UpdateSize();
        }

        private void UpdateSize()
        {
            var size = _enabled.Count + _disabled.Count;
            if (size == _size) return;
            if (size < _size)
            {
                while (size != _size)
                {
                    var stack = Instantiate(_stack, _transform, false);
                    stack.gameObject.SetActive(false);
                    _disabled.Push(stack);
                    size++;
                }
            }
            else
            {
                while (size != _size)
                {
                    Destroy(_disabled.Count > 0 ? _disabled.Pop() : _enabled.Pop());
                    size--;
                }
            }
        }
    }
}