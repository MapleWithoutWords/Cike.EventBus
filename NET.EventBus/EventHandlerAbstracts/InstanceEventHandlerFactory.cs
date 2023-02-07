using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.EventBus.EventHandlerAbstracts
{
    /// <summary>
    /// event handler instance
    /// </summary>
    public class InstanceEventHandlerFactory : IEventHandlerFactory
    {
        private readonly IEventHandler _eventHandler;

        public InstanceEventHandlerFactory(IEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public IEventHandlerDisposeWrapper GetEventHandler()
        {
            return new DefaultEventHandlerDisposeWrapper(_eventHandler);
        }
    }
}
