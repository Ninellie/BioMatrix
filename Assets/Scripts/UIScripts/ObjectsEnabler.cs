using UnityEngine;

namespace UIScripts
{
    public class ObjectsEnabler : MonoBehaviour
    {
        [SerializeField] private GameObject[] _mobileObjects;
        [SerializeField] private GameObject[] _pcObjects;

        private void Awake()
        {
            var isMobile = Application.isMobilePlatform;
            EnableObjects(isMobile);
        }

        private void EnableObjects(bool isMobile)
        {
            foreach (var mobileObject in _mobileObjects)
            {
                mobileObject.SetActive(isMobile);
            }

            foreach (var pcObject in _pcObjects)
            {
                pcObject.SetActive(!isMobile);
            }
        }
    }
}