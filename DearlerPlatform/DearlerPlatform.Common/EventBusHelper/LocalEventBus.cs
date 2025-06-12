namespace DearlerPlatform.Common.EventBusHelper;

public class LocalEventBus<T> where T : class
{
    public delegate Task EventHandler(T eventData);  // 定义委托
    private event EventHandler Handlers;
    public void Subscribe(EventHandler handler)
    {
        if (handler != null)
        {
            Handlers += handler;
        }

    }
    public void Unsubscribe(EventHandler handler)
    {
        Handlers -= handler;
    }

    public async Task PublishAsync(T eventData)
    {
        if (Handlers != null)
        {
            await Handlers.Invoke(eventData);
        }

    }
}
