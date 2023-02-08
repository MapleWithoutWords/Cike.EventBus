using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NET.EventBus.EventHandlerAbstracts;
using NET.EventBus.EventMiddleware;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NET.EventBus.LocalEvent
{
    public class LocalEventBus : EventBusBase, ILocalEventBus
    {

        public LocalEventBus(IServiceScopeFactory serviceScopeFactory, IOptions<EventBusOptions> eventOptions) : base(serviceScopeFactory, eventOptions.Value)
        {
        }



        protected override async Task PublishToEventBusAsync(EventMiddlewareContext context)
        {

            foreach (var item in context.EventHandlerFactories)
            {
                using var eventHandlerWrapper = item.GetEventHandler();

                IEventHanlderExecute execute = null;
                if (typeof(ILocalEventHandler<>).MakeGenericType(context.EventType).IsInstanceOfType(eventHandlerWrapper.EventHandler))
                {
                    //异步方法
                    //eventHandlerWrapper.EventHandler.GetType().GetMethod("HandlerAsync")?.Invoke(eventHandlerWrapper.EventHandler, new object[] { context.EventData });

                    //改成执行器，包装一层
                    execute = (IEventHanlderExecute)Activator.CreateInstance(typeof(LocalEventHanlderExecute<>).MakeGenericType(context.EventType))!;
                }

                if (typeof(IDistributedEventHandler<>).MakeGenericType(context.EventType).IsInstanceOfType(eventHandlerWrapper.EventHandler))
                {
                    execute = (IEventHanlderExecute)Activator.CreateInstance(typeof(DistributedEventHanlderExecute<>).MakeGenericType(context.EventType))!;
                }
                await execute!.ExecuteAsync(eventHandlerWrapper.EventHandler, context.EventData);
            }
        }
    }
}
