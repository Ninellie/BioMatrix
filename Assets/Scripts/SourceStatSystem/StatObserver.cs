using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.SourceStatSystem
{
    [Serializable]
    public class StatObserver
    {
        public static StatObserver CreateInstance(UnityAction<float> action, StatId statId)
        {
            return new StatObserver(action, statId);
        }

        private StatObserver(UnityAction<float> action, StatId statId)
        {
            this._id = $"{action.Method} of {action.Target} observe {statId} stat";
            this.action = action;
            this.statId = statId;
        }

        [HideInInspector] private string _id;
        public UnityAction<float> action;
        public StatId statId;
    }
}