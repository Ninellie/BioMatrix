using System;

namespace Assets.Scripts.EntityComponents
{
    [Serializable]
    public class ResourcePresetData
    {
        public string name;
        public int baseValue;
        public int minValue;
        public int edgeValue;
        public bool isLimited;
        public string maxValueStatName;
    }
}