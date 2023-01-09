using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

public class StatModifier
{
    public StatModifier(OperationType type, float value)
    {
        Type = type;
        Value = value;
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public OperationType Type { get; }
    public float Value { get; }
}