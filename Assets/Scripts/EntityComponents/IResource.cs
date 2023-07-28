using System;

namespace Assets.Scripts.EntityComponents
{
    public interface IResource
    {
        public void Set(int value);
        public void Fill();

        public void Empty();

        public void Increase(int value);

        public void Increase();

        public void Decrease(int value);

        public void Decrease();

        public int GetValue();
        public int GetMinValue();

        public float GetMaxValue();
        public int GetLackValue();
        public float GetPercentValue();
        public void SubscribeAction(ResourceEvent eventType, Action action);
        public void UnsubscribeAction(ResourceEvent eventType, Action action);
    }
}