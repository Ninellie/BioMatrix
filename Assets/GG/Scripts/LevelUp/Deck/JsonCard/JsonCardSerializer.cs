using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class JsonCardSerializer
{
    private readonly ICardRepository _cardRepository;

    public JsonCardSerializer(ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
    }

    public void Serialize(string fileName)
    {
        var list = new List<Card>();
        for (var i = 0; i < _cardRepository.CardCount; i++)
        {
            var card = _cardRepository.Get(i);
            list.Add(card);
        }

        var json = JsonConvert.SerializeObject(list.ToArray(), Formatting.Indented);
        File.WriteAllText(fileName, json);
    }
}