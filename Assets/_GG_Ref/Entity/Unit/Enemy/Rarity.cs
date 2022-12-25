using System;
using UnityEngine;

public class Rarity
{
    public RarityEnum Value { get; set; }
    public float Width =>
    Value switch
    {
        RarityEnum.Normal => 0f,
        RarityEnum.Magic => 0.02f,
        RarityEnum.Rare => 0.02f,
        RarityEnum.Unique => 0.02f,
        _ => throw new ArgumentOutOfRangeException(nameof(Value), Value, null)
    };
    public Color Color =>
        Value switch
        {
            RarityEnum.Normal => Color.white,
            RarityEnum.Magic => Color.cyan,
            RarityEnum.Rare => Color.yellow,
            RarityEnum.Unique => Color.magenta,
            _ => throw new ArgumentOutOfRangeException(nameof(Value), Value, null)
        };
    public float Multiplier =>
            Value switch
            {
                RarityEnum.Normal => 0,
                RarityEnum.Magic => 500,
                RarityEnum.Rare => 1000,
                RarityEnum.Unique => 1500,
                _ => throw new ArgumentOutOfRangeException(nameof(Value), Value, null)
            };
    public Rarity(RarityEnum value) => Value = value;
    public Rarity() => Value = RarityEnum.Normal;
}