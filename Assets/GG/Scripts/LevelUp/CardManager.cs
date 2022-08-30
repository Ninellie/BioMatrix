using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CardManager
{
    private System.Random random = new System.Random();
    private readonly Card[] cards = new Card[]
    {
        new Card
        {
            title = "Movement speed",
            description = "+ 5% to movement speed",
            dropWeight = 1000,
            improvement = new Dictionary<string, float>
            {
                ["movementSpeed"] = 100.0f,
            }
        },
        new Card
        {
            title = "Maximum HP",
            description = "+ 1 to maximum HP",
            dropWeight = 1000,
            improvement = new Dictionary<string, float>
            {
                ["maximumHP"] = 1.0f,
            }
        },
        new Card
        {
            title = "Shield",
            description = "Adds one shield to the hero that absorbs one enemy hit and goes on cooldown for 20 seconds",
            dropWeight = 100,
            improvement = new Dictionary<string, float>
            {
                ["shieldCount"] = 1.0f,
            }
        },
        new Card
        {
            title = "Regeneration",
            description = "The hero regenerates 1 HP over 15 seconds",
            dropWeight = 300,
            improvement = new Dictionary<string, float>
            {
                ["regenerationRate"] = 1.0f,
            }
        },
        new Card
        {
            title = "Fire rate",
            description = "+10 to fire rate",
            dropWeight = 1000,
            improvement = new Dictionary<string, float>
            {
                ["fireRate"] = 10.0f,
            }
        },
        new Card
        {
            title = "Projectile speed",
            description = "+100 to projectile speed",
            dropWeight = 1000,
            improvement = new Dictionary<string, float>
            {
                ["projectileSpeed"] = 100.0f,
            }
        },
        new Card
        {
            title = "Piercing projectiles",
            description = "Projectiles pierce +1 enemy",
            dropWeight = 100,
            improvement = new Dictionary<string, float>
            {
                ["pierceNumber"] = 1.0f,
            }
        },
        new Card
        {
            title = "Reload speed",
            description = "+10 to reload speed",
            dropWeight = 1000,
            improvement = new Dictionary<string, float>
            {
                ["reloadSpeed"] = 10.0f,
            }
        },
        new Card
        {
            title = "Additional projectile",
            description = "The weapon fires an additional secondary projectile",
            dropWeight = 10,
            improvement = new Dictionary<string, float>
            {
                ["projectileNumber"] = 1.0f,
            }
        },
    };
    public class Card
    {
        public string title { get; set; }
        public string description { get; set; }
        public int dropWeight { get; set; }
        public Dictionary<string, float> improvement { get; set; }
    }

    public int GetRandomCardIndex()
    {
        //var weights = cards.Select(x => x.dropWeight).ToArray();
        var sum = cards.Sum(x => x.dropWeight);
        
        var n = random.Next(sum);

        var limit = 0;

        for(int i = 0; i < cards.Length; i++)
        {
            limit += cards[i].dropWeight;
            if(n < limit)
            {
                return i;
            }
        }
        return 0;
    }

    public Card GetRandomCard() => cards[GetRandomCardIndex()];
}
