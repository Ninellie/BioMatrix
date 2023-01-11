using System;
using UnityEngine;

public class Rarity
{
    public RarityEnum Value { get; set; }
    private readonly Color _magic = new Color(0.8352942f, 0.2352941f, 0.4156863f);
    private readonly Color _rare = new Color(1f, 0.509804f, 0.454902f);
    private readonly float _normalOutline = 0.01f;

    public float Width =>
    Value switch
    {
        RarityEnum.Normal => _normalOutline,
        RarityEnum.Magic => _normalOutline,
        RarityEnum.Rare => _normalOutline,
        RarityEnum.Unique => _normalOutline,
        _ => throw new ArgumentOutOfRangeException(nameof(Value), Value, null)
    };
    public Color Color =>
        Value switch
        {
            RarityEnum.Normal => _magic,
            RarityEnum.Magic => _rare,
            RarityEnum.Rare => _magic,
            RarityEnum.Unique => Color.red,
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