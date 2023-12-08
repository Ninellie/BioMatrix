using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private List<GameObject> _enabled;
        [SerializeField] private List<GameObject> _disabled;

        private void Awake()
        {
            _transform = transform;
            UpdateSize();
        }

        private void Reset()
        {
            DisableAll();

            foreach (var disabledStack in _disabled)
            {
                Destroy(disabledStack);
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

        public void TryAdd()
        {
            var stack = _disabled.FirstOrDefault();
            if (stack == null) return;
            Add(stack);
        }

        public void TryRelease()
        {
            var stack = _enabled.FirstOrDefault();
            if (stack == null) return;
            Release(stack);
        }

        private void Add(GameObject stack)
        {
            _disabled.Remove(stack);
            stack.SetActive(true);
            _enabled.Add(stack);
        }

        private void Release(GameObject stack)
        {
            _enabled.Remove(stack);
            stack.SetActive(false);
            _disabled.Add(stack);
        }

        public void DisableAll()
        {
            foreach (var activeStack in _enabled)
            {
                activeStack.SetActive(false);
            }
            _disabled.AddRange(_enabled);
            _enabled.Clear();
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
                    _disabled.Add(stack);
                    size++;
                }
            }
            else
            {
                while (size != _size)
                {
                    if (_disabled.Count > 0)
                    {
                        var a = _disabled[0];
                        _disabled.RemoveAt(0);
                        Destroy(a);
                    }
                    else
                    {
                        var a = _enabled[0];
                        _enabled.RemoveAt(0);
                        Destroy(a);
                    }
                    size--;
                }
            }
        }
    }
}