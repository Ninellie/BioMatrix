using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameSession.Events;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class Reload : MonoBehaviour
    {
        [SerializeField] private GameObject _plateUi;
        [SerializeField] private bool _isInProgress;

        public bool IsInProcess => _isInProgress;

        private Firearm _firearm;

        private void Awake()
        {
            _firearm = GetComponent<Firearm>();
            if (_firearm.IsForPlayer)
            {
                _plateUi?.SetActive(false);
            }
        }
    
        private void OnEnable()
        {
            _firearm.Magazine.EmptyEvent += Initiate;
        }

        private void OnDisable()
        {
            _firearm.Magazine.EmptyEvent -= Initiate;
        }

        private void Initiate()
        {
            _isInProgress = true;
            _firearm.OnReload();

            if (_firearm.IsForPlayer)
            {
                _plateUi.SetActive(true);
            }

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
            _firearm.Magazine.Fill();
            _firearm.OnReloadEnd();

            if (_firearm.IsForPlayer)
            {
                _plateUi.SetActive(false);
            }
        }
    }
}
