namespace PeraCore.Runtime
{
    public interface IEventListenerBase { }

    public interface IEventListener<in T> : IEventListenerBase
    {
        void OnEvent(T e);
    }
}
