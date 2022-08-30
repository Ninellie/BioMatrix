using System.Collections.Generic;

public class Card
{
    public string title { get; set; }
    public string description { get; set; }
    public int dropWeight { get; set; }
    public Dictionary<string, float> improvement { get; set; }
}
