using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.EventBus.EventHandlerAbstracts
{
    /// <summary>
    /// IOC event handler factory
    /// </summary>
    public class IocEventHandlerFactory : IEventHandlerFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Type _eventHandlerType;

        public IocEventHandlerFactory(IServiceScopeFactory serviceScopeFactory, Type eventHandlerType)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _eventHandlerType = eventHandlerType;
        }

        public IEventHandlerDisposeWrapper GetEventHandler()
        {
            var scope = _serviceScopeFactory.CreateScope();

            // used dispose scope.
            return new DefaultEventHandlerDisposeWrapper((IEventHandler)scope.ServiceProvider.GetRequiredService(_eventHandlerType), () => scope.Dispose());
        }
    }
}
