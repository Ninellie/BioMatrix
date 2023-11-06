using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.SourceStatSystem
{
    [Serializable]
    public class StatObserver
    {
        public static StatObserver CreateInstance(UnityAction<float> action, StatId statId, int statOwnerId)
        {
            return new StatObserver(action, statId, statOwnerId);
        }

        private StatObserver(UnityAction<float> action, StatId statId, int statOwnerId)
        {
            this._id = $"{action.Method} of {action.Target} observe {statId} stat";
            this.action = action;
            this.statId = statId;
            this.statOwnerId = statOwnerId;
        }

        [HideInInspector] private string _id;
        public UnityAction<float> action;
        public int statOwnerId;
        public StatId statId;
    }
}