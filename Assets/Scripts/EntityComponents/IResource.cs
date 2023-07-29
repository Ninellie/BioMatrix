using System;

namespace Assets.Scripts.EntityComponents
{
    public interface IResource
    {
        event Action ValueChangedEvent;
        event Action IncreaseEvent;
        event Action DecreaseEvent;
        event Action IncrementEvent;
        event Action DecrementEvent;
        event Action FillEvent;
        event Action EmptyEvent;
        event Action EdgeEvent;
        event Action NotEdgeEvent;
        event Action NotEmptyEvent;
        void Set(int value);
        void Fill();
        void Empty();
        void Increase(int value);
        void Increase();
        void Decrease(int value);
        void Decrease();
        int GetValue();
        int GetMinValue();
        float GetMaxValue();
        int GetLackValue();
        float GetPercentValue();
        void SubscribeAction(ResourceEvent eventType, Action action);
        void UnsubscribeAction(ResourceEvent eventType, Action action);
    }
}