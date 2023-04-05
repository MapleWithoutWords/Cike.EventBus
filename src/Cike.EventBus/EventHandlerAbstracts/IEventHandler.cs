namespace Cike.EventBus.EventHandlerAbstracts;

/// <summary>
/// 事件处理器
/// </summary>
public interface IEventHandler
{
    /// <summary>
    /// 执行序号
    /// </summary>
    public int ExecSeqNo { get; set; }
}

/// <summary>
/// 事件处理器
/// </summary>
public interface IEventHandler<TEventData> : IEventHandler
{

    public Task HandlerAsync(TEventData eventData);
}
