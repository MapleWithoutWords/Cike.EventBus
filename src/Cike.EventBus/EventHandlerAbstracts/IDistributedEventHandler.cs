using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cike.EventBus.EventHandlerAbstracts
{
    public interface IDistributedEventHandler<TEventData> : IEventHandler
    {
        public Task HandlerAsync(TEventData eventData);
    }
}
