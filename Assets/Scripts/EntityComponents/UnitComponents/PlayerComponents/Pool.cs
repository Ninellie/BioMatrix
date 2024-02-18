using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class Pool<T> : MonoBehaviour where T : Component
    {
        public GameObject itemPrefab;
        public uint size;
        public uint maxSize;
        public ObjectPool<T> pool;
        public bool collectionCheck;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            pool = new ObjectPool<T>(
                CreateItem,
                OnGetFromPool,
                OnReleaseFromPool,
                OnItemDestroy,
                collectionCheck, (int)size, (int)maxSize);
        }

        public T Get()
        {
            return pool.Get();
        }

        public void Release(T item)
        {
            pool.Release(item);
        }

        public void Release(GameObject item)
        {
            pool.Release(item.GetComponent<T>());
        }

        private T CreateItem()
        {
            return Instantiate(itemPrefab, _transform).GetComponent<T>();
        }

        private void OnItemDestroy(T item)
        {
            Destroy(item);
        }

        private void OnGetFromPool(T item)
        {
            item.gameObject.SetActive(true);
        }

        private void OnReleaseFromPool(T item)
        {
            item.gameObject.SetActive(false);
        }
    }
}