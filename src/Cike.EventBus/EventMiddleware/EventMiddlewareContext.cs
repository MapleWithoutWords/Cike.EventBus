using Cike.EventBus.EventHandlerAbstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cike.EventBus.EventMiddleware
{
    public class EventMiddlewareContext
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public object EventData { get; set; }
        public Type EventType { get; set; }
        public ICollection<IEventHandlerFactory> EventHandlerFactories { get; set; }

        public ICollection<IEventHandlerDisposeWrapper> GetSortedEventHandler()
        {
            List<IEventHandlerDisposeWrapper> eventHandlerDisposeWrappers = new List<IEventHandlerDisposeWrapper>();
            foreach (var item in EventHandlerFactories)
            {
                eventHandlerDisposeWrappers.Add(item.GetEventHandler());
            }
            return eventHandlerDisposeWrappers.OrderBy(e => e.EventHandler.ExecSeqNo).ToList();
        }
    }
}
