using Test.Data;

namespace Test.Interfaces
{
    public interface ISubscriber
    {
        void OnEvent(CustomEventData messageData);
    }
}