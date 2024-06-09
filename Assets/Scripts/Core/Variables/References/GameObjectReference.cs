using System;
using UnityEngine;

namespace Core.Variables.References
{
    [Serializable]
    public class GameObjectReference
    {
        public bool useConstant;
        public GameObject constantValue;
        public GameObjectVariable variable;

        public GameObjectReference()
        { }

        public GameObjectReference(GameObject value)
        {
            useConstant = true;
            constantValue = value;
        }

        public GameObject Value
        {
            get => useConstant ? constantValue : variable.value;
            set
            {
                if (useConstant)
                {
                    constantValue = value;
                }
                else
                {
                    variable.value = value;
                }
            }
        }

        public static implicit operator GameObject(GameObjectReference reference)
        {
            return reference.Value;
        }
    }
}