using System;
using EntityComponents.UnitComponents.PlayerComponents;

namespace Core.Variables.References
{
    [Serializable]
    public class TransformPoolReference
    {
        public bool useConstant;
        public TransformPool constantValue;
        public TransformPoolVariable variable;

        public TransformPoolReference()
        { }

        public TransformPoolReference(TransformPool value)
        {
            useConstant = true;
            constantValue = value;
        }

        public TransformPool Value
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

        public static implicit operator TransformPool(TransformPoolReference reference)
        {
            return reference.Value;
        }
    }
}