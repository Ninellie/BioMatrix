using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EntityComponents.UnitComponents.PlayerComponents
{
    [ExecuteInEditMode]
    public class StackPool : MonoBehaviour
    {
        [SerializeField] [Range(0, 100)] private int _size;
        [SerializeField] [Range(0, 50)] private int _active;
        [SerializeField] private GameObject _stack;
        [SerializeField] private Transform _transform;
        [SerializeField] private List<GameObject> _enabled;
        [SerializeField] private List<GameObject> _disabled;
        [SerializeField] private bool _allowGettingActiveObjects;

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
            if (_transform == null) _transform = transform;
            if (_stack == null) return;
            UpdateSize();
            _active = Mathf.Clamp(_active, 0, _size);
            if (_active == _enabled.Count) return;
            UpdateEnabled();
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

        public void SetActiveAmount(int amount)
        {
            _active = Mathf.Clamp(amount, 0, _size);
            UpdateEnabled();
        }

        public GameObject Get()
        {
            var disabledObject = _disabled.FirstOrDefault();
            if (disabledObject == null)
            {
                if (!_allowGettingActiveObjects) return null;
                var enabledObject = _enabled.FirstOrDefault();
                return enabledObject != null ? enabledObject : null;
            }

            Add(disabledObject);
            return disabledObject;
        }

        public void TryAdd()
        {
            var disabledObject = _disabled.FirstOrDefault();
            if (disabledObject == null) return;
            Add(disabledObject);
        }

        public void TryRelease()
        {
            var enabledObject = _enabled.FirstOrDefault();
            if (enabledObject == null) return;
            Release(enabledObject);
        }

        public void Release(GameObject stack)
        {
            if (stack == null) return;
            if (!stack.activeInHierarchy) return;
            _enabled.Remove(stack);
            stack.SetActive(false);
            _disabled.Add(stack);
        }

        private void Add(GameObject stack)
        {
            _disabled.Remove(stack);
            stack.SetActive(true);
            _enabled.Add(stack);
        }

        private void UpdateEnabled()
        {
            var diff = _active - _enabled.Count;

            switch (diff)
            {
                case 0: return;
                case > 0:
                {
                    while (diff != 0)
                    {
                        TryAdd();
                        diff--;
                    }
                    return;
                }
                case < 0:
                {
                    while (diff != 0)
                    {
                        TryRelease();
                        diff++;
                    }
                    return;
                }
            }
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