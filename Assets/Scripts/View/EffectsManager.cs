using System;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public EffectsList PlayerEffects => _playerEffects;
    public EffectsList ShieldEffects => _shieldEffects;
    public EffectsList TurretHubEffects => _turretHubEffects;
    public EffectsList FirearmEffects => _firearmEffects;

    private EffectsList _playerEffects;
    private EffectsList _shieldEffects;
    private EffectsList _turretHubEffects;
    private EffectsList _firearmEffects;

    public void AddEffect(IEffect effect)
    {
        //var targetName = effect.Name.Split(" ")[0];
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