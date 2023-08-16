using System.Collections;
using Assets.Scripts.EntityComponents.Resources;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public class Reload : MonoBehaviour
    {
        [SerializeField] private GameObject _plateUi;
        [SerializeField] private bool _isInProgress;
        private ResourceList _resourceList;
        private Resource _magazine;
        public bool IsInProcess => _isInProgress;

        private Firearm _firearm;

        private void Awake()
        {
            _resourceList = GetComponent<ResourceList>();
            _magazine = _resourceList.GetResourceByName(ResourceName.Ammo);

            _firearm = GetComponent<Firearm>();
            if (_firearm.IsForPlayer)
                _plateUi?.SetActive(false);
        }
    
        private void OnEnable()
        {
            _magazine.GetEvent(ResourceEventType.Empty).AddListener(Initiate);
        }

        private void OnDisable()
        {
            _magazine.GetEvent(ResourceEventType.Empty).RemoveListener(Initiate);
        }

        private void Initiate()
        {
            _isInProgress = true;
            _firearm.OnReload();

            if (_firearm.IsForPlayer)
                _plateUi.SetActive(true);

            var reloadTime = _firearm.ReloadTime.Value;
            var isInstant = !(reloadTime > 0);

            switch (isInstant)
            {
                case false:
                    StartCoroutine(CoReload(reloadTime));
                    break;
                case true:
                    Complete();
                    break;
            }
        }

        private IEnumerator CoReload(float time)
        {
            yield return new WaitForSeconds(time);
            Complete();
        }

        private void Complete()
        {
            _isInProgress = false;
            _magazine.Fill();
            _firearm.OnReloadEnd();

            if (_firearm.IsForPlayer)
                _plateUi.SetActive(false);
        }
    }
}
