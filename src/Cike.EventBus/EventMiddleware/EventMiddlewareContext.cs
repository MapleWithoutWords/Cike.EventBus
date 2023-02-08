using NET.EventBus.EventHandlerAbstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.EventBus.EventMiddleware
{
    public class EventMiddlewareContext
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public object EventData { get; set; }
        public Type EventType { get; set; }
        public ICollection<IEventHandlerFactory> EventHandlerFactories { get; set; }
    }
}
