using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJson : MonoBehaviour
{

    public void SerializeTest()
    {

        var serializer = new JsonCardSerializer(new ArrayCardRepository());
        serializer.Serialize("C:\\Users\\apawl\\source\\repos\\2d_game_prototype\\Assets\\GG\\Prefab\\Cards.json");
    }
}
