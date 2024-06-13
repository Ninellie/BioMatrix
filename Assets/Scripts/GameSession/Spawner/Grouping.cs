using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace GameSession.Spawner
{
    public class Grouping
    {
        private readonly Dictionary<GroupingMode, int> _modeWeights = new()
        {
            {GroupingMode.Default, 94},
            {GroupingMode.Group, 5 },
            {GroupingMode.Surround, 1}
        };
        private int GetWeightSum()
        {
            return _modeWeights.Sum(keyValuePair => keyValuePair.Value);
        }
        public GroupingMode GetRandomMode()
        {
            var sum = GetWeightSum();

            var next = Random.Range(0, sum);

            var limit = 0;
            var listOfKeys = _modeWeights.Keys.ToList();
            var listOfValues = _modeWeights.Values.ToList();

            for (var i = 0; i < _modeWeights.Count; i++)
            {
                var groupingMode = listOfKeys[i];
                limit += listOfValues[i];
                if (next < limit)
                {
                    return groupingMode;
                }
            }
            throw new InvalidOperationException("");
        }
    }
}