using System.IO;
using Newtonsoft.Json;

public class JsonCardDeserializer
{
    public ICardRepository Deserialize(string fileName)
    {
        string json = File.ReadAllText(fileName);
        //JsonConvert.DeserializeObject<Card[]>(json);
        var cardArray = JsonConvert.DeserializeObject<Card[]>(json);
        return new ArrayCardRepository(cardArray);
    }
}