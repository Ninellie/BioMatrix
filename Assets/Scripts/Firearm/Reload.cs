using Assets.Scripts.GameSession.Events;
using Assets.Scripts.GameSession.UIScripts.View;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class Reload : MonoBehaviour
    {
        public bool IsInProcess { get; private set; }

        [SerializeField] private GameObject _plateUi;
        private Firearm _firearm;
        [SerializeField] private GameTimeScheduler _gameTimeScheduler;

        private void Awake()
        {
            _firearm = GetComponent<Firearm>();
            _gameTimeScheduler = Camera.main.GetComponent<GameTimeScheduler>();

            if (_firearm.GetIsForPlayer())
            {
                _plateUi = FindObjectOfType<ReloadPlate>().reloadPlate;
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
            IsInProcess = true;
            _firearm.OnReload();

            if (_firearm.GetIsForPlayer())
            {
                _plateUi.SetActive(true);
            }

            var reloadTime = _firearm.ReloadTime.Value;
            var isInstant = !(reloadTime > 0);

            switch (isInstant)
            {
                case false:
                    _gameTimeScheduler.Schedule(Complete, _firearm.ReloadTime.Value);
                    break;
                case true:
                    Complete();
                    break;
            }
        }

        private void Complete()
        {
            IsInProcess = false;

            if (_firearm.GetIsForPlayer())
            {
                _plateUi.SetActive(false);
            }
        
            _firearm.Magazine.Fill();

            _firearm.OnReloadEnd();
        }
    }
}
