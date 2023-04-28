using System.IO;
using UnityEngine;

public class JsonCardDeserializer
{
    public ICardRepository Deserialize(string fileName)
    {
        var json = File.ReadAllText(fileName);
        var cardArray = JsonUtility.FromJson<Card[]>(json);
        //var cardArray = JsonConvert.DeserializeObject<Card[]>(json);
        return new ArrayCardRepository(cardArray);
    }
}