using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

public class Modifier
{
    [JsonConverter(typeof(StringEnumConverter))]
    public EntityType Target { get; set; }
    public string ParameterName { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public Operation Operation { get; set; }
    public float Value { get; set; }
}