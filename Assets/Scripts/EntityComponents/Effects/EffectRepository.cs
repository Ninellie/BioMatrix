using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.EntityComponents.UnitComponents.Turret;
using Assets.Scripts.FirearmComponents;
using Assets.Scripts.GameSession.Upgrades.Deck;

namespace Assets.Scripts.EntityComponents.Effects
{

    [Serializable]
    public class EffectRepository : IEffectRepository
    {
        static EffectRepository()
        {
            IncreaseResourceOnEffects = new Dictionary<string, ResourceIncreaserOnEventOldEffect>
            {
                // Vitality Experience 2
                [nameof(Player) + "Increase" + nameof(Player.LifePoints) + 1 + nameof(Player.ExperienceTakenEvent) + 20] = new ResourceIncreaserOnEventOldEffect
                (
                    nameof(Player) + "Increase" + nameof(Player.LifePoints) + 1 + nameof(Player.ExperienceTakenEvent) + 20,
                    "",
                    nameof(Player),
                    new PropTrigger
                    {
                        Name = nameof(Player.ExperienceTakenEvent),
                        Path = ""
                    },
                    new List<(int value, string resourcePath)>
                    {
                        (1, nameof(Player.LifePoints)),
                    },
                    20,
                    false
                ),
            };
            AttachResourceIncreaserEffects = new Dictionary<string, AttachResourceIncreaserOldEffect>
            {
                // Turret
                ["PlayerTurretHubTurretsAdd1"] = new AttachResourceIncreaserOldEffect
                (
                    "PlayerTurretHubTurretsAdd1",
                    "",
                    nameof(Player),
                    new List<(int value, string resourcePath)>
                    {
                        (1, nameof(Player.TurretHub) + "." + nameof(TurretHub.Turrets)),
                    },
                    true
                ),
            };
            AttachModAdderEffects = new Dictionary<string, AttachModAdderOldEffect>
            {
                //Gun1
                ["PlayerDmgMulti50"] = new AttachModAdderOldEffect
                (
                    "PlayerDmgMulti50",
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 50f),
                            nameof(Player.Firearm) + "." + nameof(Player.Damage)),
                    }
                ),
                ["PlayerFirearmMagazineCapacityAdd2"] = new AttachModAdderOldEffect
                (
                    "PlayerFirearmMagazineCapacityAdd2",
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Addition, 2f),
                            nameof(Player.Firearm) + "." + nameof(Firearm.MagazineCapacity)),
                    }
                ),
                //Gun2
                ["PlayerFirearmShootsPerSecondMulti50Temp2"] = new AttachModAdderOldEffect
                (
                    "PlayerFirearmFirerateMulti50Temp2DurUpd",
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 50f),
                            nameof(Player.Firearm) + "." + nameof(Firearm.ShootsPerSecond)),
                    },
                    true,
                    new OldStat(2),
                    false,
                    true,
                    false,
                    false,
                    new OldStat(1, false)
                ),
                //Gun3
                ["PlayerFirearmProjectilePierceAdd2"] = new AttachModAdderOldEffect
                (
                    "PlayerFirearmProjectilePierceAdd2",
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Addition, 2f),
                            nameof(Player.Firearm) + "." + nameof(Firearm.ProjectilePierceCount)),
                    }
                ),
                ["PlayerFirearmShootForceMulti50"] = new AttachModAdderOldEffect
                (
                    "PlayerFirearmShootForceMulti50",
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 50),
                            nameof(Player.Firearm) + "." + nameof(Firearm.ShootForce)),
                    }
                ),
                //Gun Turret 2
                ["PlayerCurrentTurretFirearmShootsPerSecondMulti50"] = new AttachModAdderOldEffect
                (
                    "PlayerCurrentTurretFirearmShootsPerSecondMulti50",
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 50),
                            nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." +
                            nameof(Firearm.ShootsPerSecond)),
                    }
                ),
                //Gun Turret 3
                
                [nameof(Player) + nameof(Player.TurretHub) + nameof(TurretHub.Firearm) + nameof(Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2] = new AttachModAdderOldEffect
                //[nameof(Player) + nameof(Player.TurretHub) + nameof(TurretHub.Firearm) + nameof(Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2] = new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Player.TurretHub) + nameof(TurretHub.Firearm) + nameof(Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Addition, 2f),
                            nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." + nameof(Firearm.SingleShootProjectile)),
                    }
                ),
                //Gun Vitality 1
                [nameof(Player) + nameof(Player.MaximumLifePoints) + nameof(OperationType.Addition) + 1] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(Player.MaximumLifePoints) + nameof(OperationType.Addition) + 1,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Addition, 1f), nameof(Player.MaximumLifePoints)),
                        },
                        true
                    ),
                [nameof(Player) + nameof(Firearm) + nameof(Firearm.ProjectileSizeMultiplier) + nameof(OperationType.Addition) + 100] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(Firearm) + nameof(Firearm.ProjectileSizeMultiplier) +
                        nameof(OperationType.Addition) + 100,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Addition, 100f),
                                nameof(Player.Firearm) + "." + nameof(Firearm.ProjectileSizeMultiplier)),
                        }
                    ),
                //Gun Vitality 2
                [nameof(Player) + nameof(Player.LifeRegenerationPerSecond) + nameof(OperationType.Addition) + "1/60"] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(Player.LifeRegenerationPerSecond) + nameof(OperationType.Addition) + "1/60",
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Addition, 1 / 60f), nameof(Player.LifeRegenerationPerSecond)),
                        }
                    ),
                //Gun Vitality 3
                [nameof(Player) + nameof(Firearm) + nameof(Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 100] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(Firearm) + nameof(Firearm.ShootsPerSecond) +
                        nameof(OperationType.Multiplication) + 100,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Multiplication, 100f),
                                nameof(Player.Firearm) + "." + nameof(Firearm.ShootsPerSecond)),
                        }
                    ),
                [nameof(Player) + nameof(Firearm) + nameof(Firearm.Damage) + nameof(OperationType.Multiplication) + 50] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(Firearm) + nameof(Firearm.Damage) +
                        nameof(OperationType.Multiplication) + 50,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Multiplication, 50f),
                                nameof(Player.Firearm) + "." + nameof(Firearm.Damage)),
                        }
                    ),
                // Gun Movement Experience 1
                ["Adrenalin"] = new AttachModAdderOldEffect
                (
                    "Adrenalin",
                    "Adrenalin",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 5), nameof(Player.Speed)),
                        (new StatModifier(OperationType.Multiplication, 5), nameof(Player.Firearm) + "." + nameof(Firearm.ShootsPerSecond)),
                    },
                    true,
                    new OldStat(5, true),
                    false,
                    true,
                    true,
                    false,
                    new OldStat(10, true)
                ),
                // Gun Movement Experience 2
                [nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Multiplication, 25), nameof(Player.Speed)),
                        },
                        true
                    ),
                [nameof(Player) + nameof(Firearm) + nameof(Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 25] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(Firearm) + nameof(Firearm.ShootsPerSecond) +
                        nameof(OperationType.Multiplication) + 25,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Multiplication, 25), nameof(Player.Firearm) + "." + nameof(Firearm.ShootsPerSecond)),
                        }
                    ),
                // Gun Movement Experience 3
                [nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 25] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 25,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Multiplication, 25), nameof(Player.ExpMultiplier)),
                        },
                        true
                    ),
                //Turret 1

                //Turret 2
                [nameof(Player) + nameof(TurretHub) + nameof(Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2 + "Temp" + 2] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(TurretHub) + nameof(Firearm.SingleShootProjectile) +
                        nameof(OperationType.Addition) + 2 + "Temp" + 2,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Addition, 2f),
                                nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." +
                                nameof(Firearm.SingleShootProjectile)),
                        },
                        true,
                        new OldStat(2, true),
                        false,
                        true,
                        false,
                        false,
                        new OldStat(1, false)
                    ),
                //Turret 3
                [nameof(Player) + nameof(TurretHub) + nameof(Firearm.ProjectilePierceCount) + nameof(OperationType.Addition) + 1] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(TurretHub) + nameof(Firearm.ProjectilePierceCount) +
                        nameof(OperationType.Addition) + 1,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Addition, 1),
                                nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." +
                                nameof(Firearm.ProjectilePierceCount)),
                        }
                    ),
                [nameof(Player) + nameof(TurretHub) + nameof(Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 50] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(TurretHub) + nameof(Firearm.ShootsPerSecond) +
                        nameof(OperationType.Multiplication) + 50,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Multiplication, 50),
                                nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." +
                                nameof(Firearm.ShootsPerSecond)),
                        }
                    ),
                // Turret Shield 1
                [nameof(Player) + nameof(Shield) + nameof(Shield.MaxRechargeableLayers) + nameof(OperationType.Addition) + 1] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(Shield) + nameof(Shield.MaxRechargeableLayers) + nameof(OperationType.Addition) + 1,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Addition, 1), nameof(Player.Shield) + "." + nameof(Shield.MaxRechargeableLayers)),
                        },
                        true
                    ),
                // Turret Shield 2
                [nameof(Player) + nameof(Shield) + nameof(Shield.RechargeRatePerSecond) + nameof(OperationType.Multiplication) + 25] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(Shield) + nameof(Shield.RechargeRatePerSecond) + nameof(OperationType.Multiplication) + 25,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Multiplication, 25), nameof(Player.Shield) + "." + nameof(Shield.RechargeRatePerSecond)),
                        },
                        true
                    ),
                // Turret Shield 3
                [nameof(Player) + nameof(TurretHub) + nameof(Firearm.Damage) + nameof(OperationType.Multiplication) + 50] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(TurretHub) + nameof(Firearm.Damage) +
                        nameof(OperationType.Multiplication) + 50,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Multiplication, 50),
                                nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." +
                                nameof(Firearm.Damage)),
                        }
                    ),
                // Turret Movement Magnetism 1
                [nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25] =
                    new AttachModAdderOldEffect
                    (
                        nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25,
                        "",
                        nameof(Player),
                        new List<(StatModifier mod, string statPath)>
                        {
                            (new StatModifier(OperationType.Multiplication, 25), nameof(Player.MagnetismRadius)),
                        },
                        true
                    ),
                // Vitality Experience 1

                // Vitality Experience 3
                //[nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 100] =
                //    new AttachModAdderEffect
                //    (
                //        nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 100,
                //        "",
                //        nameof(Player),
                //        new List<(StatModifier mod, string statPath)>
                //        {
                //            (new StatModifier(OperationType.Multiplication, 100), nameof(Player.ExpMultiplier)),
                //        },
                //        false
                //    ),

            };
            ToggleEffects = new Dictionary<string, ToggleOnAttach>
            {
                //Gun Turret 2
                ["PlayerIsSameTurretTargetTrue"] = new ToggleOnAttach
                (
                    "PlayerIsSameTurretTargetTrue",
                    "",
                    nameof(Player),
                    nameof(Player.TurretHub) + "." + nameof(TurretHub.IsSameTurretTarget),
                    true
                ),
            };
            EffectAdderOnEventEffects = new Dictionary<string, OldEffectAdderOnEventOldEffect>
            {
                // Gun 2
                ["PlayerFirearmReloadEndEventPlayerFirearmShootsPerSecondMulti50Temp2"] = new OldEffectAdderOnEventOldEffect
                (
                    "PlayerFirearmReloadEndEventPlayerFirearmShootsPerSecondMulti50Temp2",
                    "",
                    nameof(Player),
                    new PropTrigger
                    {
                        Name = nameof(Firearm.ReloadEndEvent),
                        Path = nameof(Player.Firearm)
                    },
                    AttachModAdderEffects["PlayerFirearmShootsPerSecondMulti50Temp2"]
                ),
                // Gun Movement Experience 1
                [nameof(Player) + nameof(Player.ExperienceTakenEvent) + "Adrenalin"] = new OldEffectAdderOnEventOldEffect
                (
                    nameof(Player) + nameof(Player.ExperienceTakenEvent) + "Adrenalin",
                    "Adrenalin's source",
                    nameof(Player),
                    new PropTrigger
                    {
                        Name = nameof(Player.ExperienceTakenEvent),
                        Path = ""
                    },
                    AttachModAdderEffects["Adrenalin"]
                ),
                // Turret 2
                [nameof(Player) + nameof(TurretHub.KillsCount.IncrementEvent) + nameof(Player) + nameof(TurretHub) + nameof(Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2 + "Temp" + 2] =
                    new OldEffectAdderOnEventOldEffect
                    (
                        nameof(Player) + nameof(TurretHub.KillsCount.IncrementEvent) + nameof(Player) + nameof(TurretHub) +
                        nameof(Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2 + "Temp" + 2,
                        "",
                        nameof(Player),
                        new PropTrigger
                        {
                            Name = nameof(TurretHub.KillsCount.IncrementEvent),
                            Path = nameof(Player.TurretHub) + "." + nameof(TurretHub.KillsCount)
                        },
                        AttachModAdderEffects[
                            nameof(Player) + nameof(TurretHub) + nameof(Firearm.SingleShootProjectile) +
                            nameof(OperationType.Addition) + 2 + "Temp" + 2]
                    ),
            };
            EffectAdderWhileTrueEffects = new Dictionary<string, OldEffectAdderWhileTrueOldEffect>
            {
                // Gun Turret 2
                ["GunTurret2_While"] = new OldEffectAdderWhileTrueOldEffect
                (
                    "GunTurret2_While",
                    "",
                    nameof(Player),
                    new PropTrigger
                    {
                        Name = nameof(Player.FireEvent),
                        Path = ""
                    },
                    new PropTrigger
                    {
                        Name = nameof(Player.FireOffEvent),
                        Path = ""
                    },
                    nameof(Player.IsFireButtonPressed),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (ToggleEffects["PlayerIsSameTurretTargetTrue"], 1),
                    }
                    
                ),
                // Gun Turret 3
                ["GunTurret3_While"] = new OldEffectAdderWhileTrueOldEffect
                (
                    "GunTurret3_While",
                    "While magazine full all turrets get +2 projectiles",
                    nameof(Player),
                    new PropTrigger
                    {
                        //Name = nameof(Firearm.Magazine.FillEvent),
                        Name = nameof(ResourceEvent.FillEvent),
                        Path = nameof(Player.Firearm) + "." + nameof(Firearm.Magazine)
                    },
                    new PropTrigger
                    {
                        //Name = nameof(Firearm.Magazine.DecreaseEvent),
                        Name = nameof(ResourceEvent.DecreaseEvent),
                        Path = nameof(Player.Firearm) + "." + nameof(Firearm.Magazine)
                    },
                    nameof(Player.Firearm) + "." +
                    nameof(Firearm.Magazine) + "." +
                    nameof(OldResource.IsFull),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (AttachModAdderEffects[nameof(Player) + nameof(Player.TurretHub) + nameof(TurretHub.Firearm) + nameof(Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2], 1)
                    }
                ),
                // Gun Vitality 3
                ["GunVitality3_While_1"] = new OldEffectAdderWhileTrueOldEffect
                (
                    "GunVitality3_While_1",
                    "While life points is on edge add +100% firerate multi",
                    nameof(Player),
                    new PropTrigger
                    {
                        Name = nameof(OldResource.EdgeEvent),
                        Path = nameof(Player.LifePoints)
                    },
                    new PropTrigger
                    {
                        Name = nameof(OldResource.NotEdgeEvent),
                        Path = nameof(Player.LifePoints)
                    },
                    nameof(Player.LifePoints) + "." + nameof(OldResource.IsOnEdge),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (AttachModAdderEffects[nameof(Player) + nameof(Firearm) + nameof(Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 100], 1)
                    }
                ),
                ["GunVitality3_While_2"] = new OldEffectAdderWhileTrueOldEffect
                (
                    "GunVitality3_While_2",
                    "While life points is on edge add +50% damage multi",
                    nameof(Player),
                    new PropTrigger
                    {
                        Name = nameof(OldResource.EdgeEvent),
                        Path = nameof(Player.LifePoints)
                    },
                    new PropTrigger
                    {
                        Name = nameof(OldResource.NotEdgeEvent),
                        Path = nameof(Player.LifePoints)
                    },
                    nameof(Player.LifePoints) + "." + nameof(OldResource.IsOnEdge),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (AttachModAdderEffects[nameof(Player) + nameof(Firearm) + nameof(Firearm.Damage) +nameof(OperationType.Multiplication) + 50], 1)
                    }
                ),
                // Turret Shield 3
                ["TurretShield3_While_1"] = new OldEffectAdderWhileTrueOldEffect
                (
                    "TurretShield3_While_1",
                    "+ 1 turret while has no shield",
                    nameof(Player),
                    new PropTrigger
                    {
                        Name = nameof(OldResource.EmptyEvent),
                        Path = nameof(Player.Shield) + "." + nameof(Shield.LayersCount)
                    },
                    new PropTrigger
                    {
                        Name = nameof(OldResource.NotEmptyEvent),
                        Path = nameof(Player.Shield) + "." + nameof(Shield.LayersCount)
                    },
                    nameof(Player.Shield) + "." + nameof(Shield.LayersCount) + "." + nameof(OldResource.IsEmpty),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (AttachResourceIncreaserEffects["PlayerTurretHubTurretsAdd1"], 1)
                    }
                    
                ),
                // Turret Shield 3
                ["TurretShield3_While_2"] = new OldEffectAdderWhileTrueOldEffect
                (
                    "TurretShield3_While_2",
                    "+ 50 turret damage multi while has shield",
                    nameof(Player),
                    new PropTrigger
                    {
                        Name = nameof(OldResource.NotEmptyEvent),
                        Path = nameof(Player.Shield) + "." + nameof(Shield.LayersCount)
                    },
                    new PropTrigger
                    {
                        Name = nameof(OldResource.EmptyEvent),
                        Path = nameof(Player.Shield) + "." + nameof(Shield.LayersCount)
                    },
                    nameof(Player.Shield) + "." + nameof(Shield.LayersCount) + "." + nameof(OldResource.IsNotEmpty),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (AttachModAdderEffects[nameof(Player) + nameof(TurretHub) + nameof(Firearm.Damage) + nameof(OperationType.Multiplication) + 50], 1)
                    }
                ),
                // Vitality Experience 3
                ["VitalityExperience3_While_1"] = new OldEffectAdderWhileTrueOldEffect
                (
                    "VitalityExperience3_While_1",
                    "While life points is on edge add +100% experience gain multi",
                    nameof(Player),
                    new PropTrigger
                    {
                        Name = nameof(OldResource.EdgeEvent),
                        Path = nameof(Player.LifePoints)
                    },
                    new PropTrigger
                    {
                        Name = nameof(OldResource.NotEdgeEvent),
                        Path = nameof(Player.LifePoints)
                    },
                    nameof(Player.LifePoints) + "." + nameof(OldResource.IsOnEdge),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (AttachModAdderEffects[nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 25], 4)
                    }
                ),
                // Shield Magnetism 3
                ["ShieldMagnetism3_While_1"] = new OldEffectAdderWhileTrueOldEffect
                (
                    "ShieldMagnetism3_While_1",
                    "+100% magnetism radius while shield is fully charged",
                    nameof(Player),
                    new PropTrigger
                    {
                        Name = nameof(OldRecoverableResource.FullRecoveryEvent),
                        Path = nameof(Player.Shield) + "." + nameof(Shield.LayersCount)
                    },
                    new PropTrigger
                    {
                        Name = nameof(OldRecoverableResource.RecoveryStartEvent),
                        Path = nameof(Player.Shield) + "." + nameof(Shield.LayersCount)
                    },
                    nameof(Player.Shield) + "." + nameof(Shield.LayersCount) + "." + nameof(OldRecoverableResource.IsFullyRecovered),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (AttachModAdderEffects[nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25], 4)
                    }
                ),
            };
            EffectAdderPerResourceEffects = new Dictionary<string, OldEffectAdderPerResource>
            {
                // Turret Movement Magnetism 3
                ["25msPerTurret"] = new OldEffectAdderPerResource
                (
                    "25msPerTurret",
                    "",
                    nameof(Player),
                    AttachModAdderEffects[nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25],
                    new PropTrigger
                    {
                        Name = nameof(Player.TurretHub.Turrets.ValueChangedEvent),
                        Path = nameof(Player.TurretHub) + "." + nameof(TurretHub.Turrets)
                    }
                )
            };
            CardEffects = new Dictionary<string, AttachOldEffectAdderOldEffect>
            {
                // Gun
                [nameof(CardTag.Gun) + 1] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + 1,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects["PlayerDmgMulti50"], stackCount: 1),
                        (effect: AttachModAdderEffects["PlayerFirearmMagazineCapacityAdd2"], stackCount: 1)
                    }
                ),
                [nameof(CardTag.Gun) + 2] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + 2,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: EffectAdderOnEventEffects["PlayerFirearmReloadEndEventPlayerFirearmShootsPerSecondMulti50Temp2"], stackCount: 1)
                    }
                ),
                [nameof(CardTag.Gun) + 3] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + 3,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects["PlayerFirearmProjectilePierceAdd2"], stackCount: 1),
                        (effect: AttachModAdderEffects["PlayerFirearmShootForceMulti50"], stackCount: 1)
                    }
                ),
                // Gun Turret
                [nameof(CardTag.Gun) + nameof(CardTag.Turret) + 1] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + nameof(CardTag.Turret) + 1,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachResourceIncreaserEffects["PlayerTurretHubTurretsAdd1"], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Gun) + nameof(CardTag.Turret) + 2] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + nameof(CardTag.Turret) + 2,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: EffectAdderWhileTrueEffects["GunTurret2_While"], stackCount: 1),
                        (effect: AttachModAdderEffects["PlayerCurrentTurretFirearmShootsPerSecondMulti50"], stackCount: 1)
                    }
                ),
                [nameof(CardTag.Gun) + nameof(CardTag.Turret) + 3] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + nameof(CardTag.Turret) + 3,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: EffectAdderWhileTrueEffects["GunTurret3_While"], stackCount: 1)
                    }
                ),
                // Gun Vitality
                [nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 1] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 1,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MaximumLifePoints) + nameof(OperationType.Addition) + 1], stackCount: 1),
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Firearm) + nameof(Firearm.ProjectileSizeMultiplier) + nameof(OperationType.Addition) + 100], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 2] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 2,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.LifeRegenerationPerSecond) + nameof(OperationType.Addition) + "1/60"], stackCount: 1)
                    }
                ),
                [nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 3] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 3,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: EffectAdderWhileTrueEffects["GunVitality3_While_1"], stackCount: 1),
                        (effect: EffectAdderWhileTrueEffects["GunVitality3_While_2"], stackCount: 1)
                    }
                ),
                // Gun Movement Experience
                [nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 1] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 1,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: EffectAdderOnEventEffects[nameof(Player) + nameof(Player.ExperienceTakenEvent) + "Adrenalin"], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 2] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 2,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Firearm) + nameof(Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 3] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 3,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 25], stackCount: 4),
                    }
                ),
                // Turret
                [nameof(CardTag.Turret) + 1] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Turret) + 1,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachResourceIncreaserEffects["PlayerTurretHubTurretsAdd1"], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Turret) + 2] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Turret) + 2,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: EffectAdderOnEventEffects[nameof(Player) + nameof(TurretHub.KillsCount.IncrementEvent) + nameof(Player) + nameof(TurretHub) + nameof(Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2 + "Temp" + 2], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Turret) + 3] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Turret) + 3,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(TurretHub) + nameof(Firearm.ProjectilePierceCount) + nameof(OperationType.Addition) + 1], stackCount: 1),
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(TurretHub) + nameof(Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 50], stackCount: 1),
                    }
                ),
                // Turret Shield
                [nameof(CardTag.Turret) + nameof(CardTag.Shield) + 1] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Turret) + nameof(CardTag.Shield) + 1,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Shield) + nameof(Shield.MaxRechargeableLayers) + nameof(OperationType.Addition) + 1], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Turret) + nameof(CardTag.Shield) + 2] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Turret) + nameof(CardTag.Shield) + 2,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Shield) + nameof(Shield.RechargeRatePerSecond) + nameof(OperationType.Multiplication) + 25], stackCount: 4),
                    }
                ),
                [nameof(CardTag.Turret) + nameof(CardTag.Shield) + 3] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Turret) + nameof(CardTag.Shield) + 3,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: EffectAdderWhileTrueEffects["TurretShield3_While_1"], stackCount: 1),
                        (effect: EffectAdderWhileTrueEffects["TurretShield3_While_2"], stackCount: 1),
                    }
                ),
                // Turret Movement Magnetism
                [nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 1] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 1,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: EffectAdderPerResourceEffects["25msPerTurret"], stackCount: 1),
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 2] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 2,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 3] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 3,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachResourceIncreaserEffects["PlayerTurretHubTurretsAdd1"], stackCount: 1),
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25], stackCount: 2),
                    }
                ),
                // Vitality Experience
                [nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 1] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 1,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MaximumLifePoints) + nameof(OperationType.Addition) + 1], stackCount: 1),
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 2] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 2,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: IncreaseResourceOnEffects[nameof(Player) + "Increase" + nameof(Player.LifePoints) + 1 + nameof(Player.ExperienceTakenEvent) + 20], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 3] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 3,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: EffectAdderWhileTrueEffects["VitalityExperience3_While_1"], stackCount: 1),
                    }
                ),
                // Shield Magnetism
                [nameof(CardTag.Shield) + nameof(CardTag.Magnetism) + 1] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Shield) + nameof(CardTag.Magnetism) + 1,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Shield) + nameof(Shield.RechargeRatePerSecond) + nameof(OperationType.Multiplication) + 25], stackCount: 2)
                    }
                ),
                [nameof(CardTag.Shield) + nameof(CardTag.Magnetism) + 2] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Shield) + nameof(CardTag.Magnetism) + 2,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Shield) + nameof(Shield.MaxRechargeableLayers) + nameof(OperationType.Addition) + 1], stackCount: 1),
                        (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                    }
                ),
                [nameof(CardTag.Shield) + nameof(CardTag.Magnetism) + 3] = new AttachOldEffectAdderOldEffect
                (
                    nameof(CardTag.Shield) + nameof(CardTag.Magnetism) + 3,
                    "",
                    nameof(Player),
                    new List<(IOldEffect effect, int stackCount)>
                    {
                        (effect: EffectAdderWhileTrueEffects["ShieldMagnetism3_While_1"], stackCount: 1),
                    }
                ),
            };
        }

        private static readonly Dictionary<string, ResourceIncreaserOnEventOldEffect> IncreaseResourceOnEffects;
        
        private static readonly Dictionary<string, AttachResourceIncreaserOldEffect> AttachResourceIncreaserEffects;

        private static readonly Dictionary<string, AttachModAdderOldEffect> AttachModAdderEffects;

        private static readonly Dictionary<string, ToggleOnAttach> ToggleEffects;

        private static readonly Dictionary<string, OldEffectAdderOnEventOldEffect> EffectAdderOnEventEffects;

        private static readonly Dictionary<string, OldEffectAdderWhileTrueOldEffect> EffectAdderWhileTrueEffects;

        private static readonly Dictionary<string, OldEffectAdderPerResource> EffectAdderPerResourceEffects;
        
        private static readonly Dictionary<string, AttachOldEffectAdderOldEffect> CardEffects;

        public IOldEffect Get(string effectName)
        {
            return CardEffects[effectName];
        }
    }
}