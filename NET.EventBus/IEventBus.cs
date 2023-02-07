using NET.EventBus.EventHandlerAbstracts;

namespace NET.EventBus
{
    public interface IEventBus
    {
        /// <summary>
        /// publish a event
        /// </summary>
        /// <param name="eventData">event data</param>
        /// <param name="isReleased"> is release event? false:add to queue.true:relaesed</param>
        /// <returns></returns>
        public Task PublishAsync(Type eventType, object eventData, bool isReleased = false);

        /// <summary>
        /// publish a event
        /// </summary>
        /// <param name="eventData">event data</param>
        /// <param name="isReleased"> is release event? false:add to queue.true:relaesed</param>
        /// <returns></returns>
        public Task PublishAsync<TEvent>(TEvent eventData, bool isReleased = false);

        /// <summary>
        /// subscribe a handler for event
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <returns></returns>
        public IDisposable Subscribe<TEventData, TEventHandler>()
                                where TEventData : class
                                where TEventHandler : class, IEventHandler, new();

        /// <summary>
        ///  subscribe a handler for event
        /// </summary>
        /// <param name="eventDataType"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public IDisposable Subscribe(Type eventDataType, IEventHandler eventHandler);
        public IDisposable Subscribe<TEventData>(IEventHandler eventHandler);

        public IDisposable Subscribe(Type eventDataType, IEventHandlerFactory eventHandlerFactory);
        public IDisposable Subscribe<TEventData>(IEventHandlerFactory eventHandlerFactory);
        /// <summary>
        /// unsubscribe a handler for event
        /// </summary>
        /// <param name="eventDataType"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public void UnSubscribe(Type eventDataType, IEventHandler eventHandler);
        public void UnSubscribe<TEventData>(IEventHandler eventHandler);
        public void UnSubscribe(Type eventDataType, IEventHandlerFactory eventHandler);
        public void UnSubscribe<TEventData>(IEventHandlerFactory eventHandlerFactory);

        /// <summary>
        /// un subscribe all handler for event
        /// </summary>
        /// <param name="eventDataType"></param>
        /// <returns></returns>
        public void UnSubscribeAll(Type eventDataType);

    }
}