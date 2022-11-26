using System.IO;
using Newtonsoft.Json;

public class JsonCardDeserializer
{
    public ICardRepository Deserialize(string fileName)
    {
        var json = File.ReadAllText(fileName);
        var cardArray = JsonConvert.DeserializeObject<Card[]>(json);
        return new ArrayCardRepository(cardArray);
    }
}