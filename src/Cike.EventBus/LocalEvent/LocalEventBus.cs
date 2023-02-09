using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Cike.EventBus.EventHandlerAbstracts;
using Cike.EventBus.EventMiddleware;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cike.EventBus.LocalEvent
{
    public class LocalEventBus : EventBusBase, ILocalEventBus
    {

        public LocalEventBus(IServiceScopeFactory serviceScopeFactory, IOptions<EventBusOptions> eventOptions) : base(serviceScopeFactory, eventOptions.Value)
        {
        }


    }
}
