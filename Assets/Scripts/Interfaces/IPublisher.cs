using Test.Data;

namespace Test.Interfaces
{
    public interface IPublisher
    {
        void Publish(CustomEventData data);
        void AddSubscriber(GameEventName eventName, ISubscriber subscriber);
        void RemoveSubscriber(GameEventName eventName, ISubscriber subscriber);
    }
}