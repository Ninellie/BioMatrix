using System;
using Assets.Scripts.EntityComponents.Effects;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private EffectsList _playerEffects;
    [SerializeField] private EffectsList _shieldEffects;
    [SerializeField] private EffectsList _turretHubEffects;
    [SerializeField] private EffectsList _firearmEffects;

    public void AddEffect(IEffect effect)
    {
        var targetName = effect.TargetName;
        switch (targetName)
        {
            case TargetName.Player:
                _playerEffects.AddEffect(effect);
                break;
            case TargetName.Shield:
                _shieldEffects.AddEffect(effect);
                break;
            case TargetName.TurretHubWeapon:
                _turretHubEffects.AddEffect(effect);
                break;
            case TargetName.Firearm:
                _firearmEffects.AddEffect(effect);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}