namespace GameCore.EventBus.Messaging
{
    public interface IEventReceiver<T> : IBaseEventReceiver where T : struct, IEvent
    {
        void OnEvent(T eventMessage);
    }
}