using System.ComponentModel;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static System.Enum;

//public class UnitParameters
//{
//    List<UnitParameter> unitParameters = new List<UnitParameter>() { UnitParameter.Size, UnitParameter.Speed, UnitParameter.HealthPoints};
//    public float Size { get; set; }
//    public float Speed { get; set; }
//    public int HealthPoints { get; set; }
//}
//public class Player_ : Unit
//{
//    //public PlayerParameter[] PlayerParameters;
//}
//public class Enemy_ : Unit
//{

//}
//public class Projectile_ : Unit
//{

//}

//public class Unit
//{
//    public EntityType name;
//    public Setting[] parameters = new Setting[]
//    {
//        new Setting
//        {
//            name = "Size",
//            type = float,
//        },
//    };
//}
////public class UnitParameter
////{
////    public Type GetType(UnitParameterEnum parameter)
////    {
////        switch (parameter)
////        {
////            case UnitParameterEnum.Size:
////                return typeof(float);
////            case UnitParameterEnum.Speed:
////                return typeof(float);
////            case UnitParameterEnum.HealthPoints:
////                return typeof(int);

////        }
////    }
////}
//public enum UnitParameterEnum
//{
//    Size,
//    Speed,
//    MaximumHP,
//}

//public enum PlayerParameter
//{
//    Level,
//    Experience,
//    Weapon,
//}
//public enum WeaponParameter
//{
//    ShootForce,
//    ShotsPerSecond,
//    MaxShootDeflectionAngle,
//    MagazineSize,
//    ReloadSpeed,
//    SingleShotProjectiles,
//    Ammo,
//}
//public enum AmmoParameter
//{
//    PierceCount
//}

public class Card
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int DropWeight { get; set; }
    public Modifier[] ModifierList { get; set; }
}
