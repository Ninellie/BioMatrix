using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    /// <summary>
    /// Game Mode Component Only
    /// </summary>
    public class GameObjectPool : MonoBehaviour
    {
        public GameObject itemPrefab;
        public uint size;
        public uint maxSize;

        private Transform _transform;
        private ObjectPool<GameObject> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<GameObject>(
                CreateItem,
                OnGetFromPool,
                OnReleaseFromPool,
                OnItemDestroy,
                true, (int)size, (int)maxSize);
        }

        private GameObject CreateItem()
        {
            return Instantiate(itemPrefab, _transform);
        }

        private void OnItemDestroy(GameObject item)
        {
            Destroy(item);
        }

        private void OnGetFromPool(GameObject item)
        {
            item.SetActive(true);
        }

        private void OnReleaseFromPool(GameObject item)
        {
            item.SetActive(false);
        }
    }
}