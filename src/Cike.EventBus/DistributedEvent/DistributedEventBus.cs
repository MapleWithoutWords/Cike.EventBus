using NET.EventBus.LocalEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.EventBus.DistributedEvent
{
    public class DistributedEventBus : IDistributedEventBus
    {
        private readonly ILocalEventBus _localEventBus;

        public DistributedEventBus(ILocalEventBus localEventBus)
        {
            _localEventBus = localEventBus;
        }

        public async Task PublishAsync<TEvent>(TEvent eventData, bool isComplete = false)
        {
            await _localEventBus.PublishAsync(eventData, isComplete);
        }
    }
}
