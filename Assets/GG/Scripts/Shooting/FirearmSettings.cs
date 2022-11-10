using UnityEngine;

public class FirearmSettings : MonoBehaviour
{
    [SerializeField] private float shootForce;
    [SerializeField] private float shotsPerSecond;
    [SerializeField] private float maxShotDeflectionAngle;
    [SerializeField] private int magazineSize;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private int singleShotProjectiles;
    [SerializeField] private GameObject ammo;

    public float ShootForce => shootForce;
    public float ShotsPerSecond => shotsPerSecond;
    public float MaxShotDeflectionAngle => maxShotDeflectionAngle;
    public int MagazineSize => magazineSize;
    public float ReloadSpeed => reloadSpeed;
    public int SingleShotProjectiles => singleShotProjectiles;
    public GameObject Ammo => ammo;
}