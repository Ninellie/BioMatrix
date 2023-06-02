using System;

[Flags]
public enum CardTag
{
    Gun = 1,
    Turret = 2,
    Vitality = 4,
    Shield = 8,
    Movement = 16,
    Magnetism = 32,
    Experience = 64,
}