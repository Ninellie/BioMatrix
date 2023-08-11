using System;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public EffectsList PlayerEffects => _playerEffects;
    public EffectsList ShieldEffects => _shieldEffects;
    public EffectsList TurretHubEffects => _turretHubEffects;
    public EffectsList FirearmEffects => _firearmEffects;

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
            case TargetName.TurretHub:
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