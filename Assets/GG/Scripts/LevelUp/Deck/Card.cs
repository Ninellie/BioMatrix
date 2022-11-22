public class Card
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int DropWeight { get; set; }
    public string[] InfluencedStats { get; set; }
    public StatModifier[] ModifierList { get; set; }
}
