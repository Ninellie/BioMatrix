using System;

[Serializable]
public class Card
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int DropWeight { get; set; }
    public IEffect[] Effects { get; set; }
    //public CardTag[] Tags { get; set; } 
    //public string[][] Effects1 { get; set; }
}