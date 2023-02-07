using NET.EventBus.EventHandlerAbstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.EventBus
{
    public interface IDistributedEventBus : IEventBus
    {
        public Task Subscribe<TEventData>(IDistributedEventHandler<TEventData> handler);
    }
}
