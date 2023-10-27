using Assets.Scripts.EntityComponents.Resources;

public interface IWeapon
{
    Resource GetAmmoResource();
    void DoAction();
}