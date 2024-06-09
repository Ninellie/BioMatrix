using Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    public class ShieldAnimator : MonoBehaviour
    {
        [ContextMenuItem(nameof(UpdateAnimation), nameof(UpdateAnimation))]
        [SerializeField] private Animator _animator;
        [ContextMenuItem(nameof(PlayIdle1), nameof(PlayIdle1))]
        [SerializeField] private string _idle1;
        [ContextMenuItem(nameof(PlayIdle2), nameof(PlayIdle2))]
        [SerializeField] private string _idle2;
        [ContextMenuItem(nameof(PlayIdle3), nameof(PlayIdle3))]
        [SerializeField] private string _idle3;
        [ContextMenuItem(nameof(PlayBlast), nameof(PlayBlast))]
        [SerializeField] private string _blast;
        [SerializeField] private IntReference _layers;

        private void Start()
        {
            UpdateAnimation();
        }

        public void UpdateAnimation()
        {
            if (_layers == 0)
                PlayBlast();
            if (_layers == 1)
                PlayIdle1();
            if (_layers == 2)
                PlayIdle2();
            if (_layers > 2)
                PlayIdle3();
        }

        private void PlayBlast() => _animator.Play(_blast);
        private void PlayIdle1() => _animator.Play(_idle1);
        private void PlayIdle2() => _animator.Play(_idle2);
        private void PlayIdle3() => _animator.Play(_idle3);
    }
}