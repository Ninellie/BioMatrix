using System;

namespace Assets.Scripts.Core.Variables.References
{
    [Serializable]
    public class StatReference
    {
        public bool useConstant;
        public float constantValue;
        public StatVariable variable;

        public StatReference()
        { }

        public StatReference(float value)
        {
            useConstant = true;
            constantValue = value;
        }

        public float Value => useConstant ? constantValue : variable.value;

        public static implicit operator float(StatReference reference)
        {
            return reference.Value;
        }
    }
}