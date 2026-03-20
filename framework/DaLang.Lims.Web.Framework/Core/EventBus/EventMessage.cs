namespace DaLang.Lims.Web.Framework.Core.EventBus;

public class EventMessage<T> where T : new()
{
    public T Data { get; set; } = new T();
}
