using NET.EventBus.EventHandlerAbstracts;

namespace NET.EventBus
{
    public interface IEventBus
    {

        /// <summary>
        /// publish a event
        /// </summary>
        /// <param name="eventData">event data</param>
        /// <param name="isComplete"> is release event? false:add to queue.true:relaesed</param>
        /// <returns></returns>
        public Task PublishAsync<TEvent>(TEvent eventData, bool isComplete = false);

    }
}