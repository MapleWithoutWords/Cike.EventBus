using Microsoft.Extensions.DependencyInjection;
using Cike.EventBus.EventHandlerAbstracts;
using Cike.EventBus.EventMiddleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cike.EventBus
{
    public abstract class EventBusBase : IEventBus
    {
        protected IServiceScopeFactory ServiceScopeFactory { get; }

        private IDictionary<Type, List<IEventHandlerFactory>> _eventHandlerFactoryDic;

        private EventMiddlewareDelegate _eventDelegate;

        protected EventBusBase(IServiceScopeFactory serviceScopeFactory, EventBusOptions eventBusOptions)
        {
            ServiceScopeFactory = serviceScopeFactory;

            SubscribeOptionsHanlder(eventBusOptions.EventMiddlewares);

            CreateDelegate(serviceScopeFactory, eventBusOptions.EventMiddlewares);
        }


        /// <inheritdoc/>
        public async Task PublishAsync<TEvent>(TEvent eventData, bool isComplete = false)
        {
            if (isComplete == false)
            {
                //TODO: unit of work
            }
            var context = GetEventMiddlewareContext(typeof(TEvent), eventData);
            await _eventDelegate.Invoke(context);
        }

        protected virtual async Task PublishToEventBusAsync(EventMiddlewareContext context)
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

        private EventMiddlewareContext GetEventMiddlewareContext(Type eventType, object eventData)
        {
            EventMiddlewareContext context = new EventMiddlewareContext()
            {
                EventData = eventData,
                EventType = eventType,
                EventHandlerFactories = GetEventHandlerFactories(eventType),
            };
            return context;
        }

        /// <summary>
        /// 获取事件处理器
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public virtual ICollection<IEventHandlerFactory> GetEventHandlerFactories(Type eventType)
        {
            List<IEventHandlerFactory> factories = new List<IEventHandlerFactory>();
            foreach (var item in _eventHandlerFactoryDic)
            {
                //当前类型，或者其子类的处理器
                if (item.Key == eventType || eventType.IsAssignableFrom(item.Key))
                {
                    factories.AddRange(item.Value);
                }
            }
            return factories;
        }

        /// <summary>
        /// 订阅配置中的handler
        /// </summary>
        /// <param name="eventHanlders"></param>
        private void SubscribeOptionsHanlder(IEnumerable<Type> eventHanlders)
        {
            foreach (var itemHanlderType in eventHanlders)
            {
                if (typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(itemHanlderType) == false)
                {
                    continue;
                }
                var handlerInterfaces = itemHanlderType.GetInterfaces();

                foreach (var handlerInterface in handlerInterfaces)
                {
                    if (handlerInterface.IsGenericType == false)
                    {
                        continue;
                    }
                    var genericArgs = handlerInterface.GetGenericArguments();
                    if (genericArgs.Length == 1)
                    {
                        Subscribe(genericArgs[0], new IocEventHandlerFactory(ServiceScopeFactory, itemHanlderType));
                    }
                }
            }
        }

        public virtual void Subscribe(Type eventDataType, IEventHandlerFactory eventHandlerFactory)
        {
            if (_eventHandlerFactoryDic.ContainsKey(eventDataType) == false)
            {
                _eventHandlerFactoryDic[eventDataType] = new List<IEventHandlerFactory>();
            }
            _eventHandlerFactoryDic[eventDataType].Add(eventHandlerFactory);
        }

        private void CreateDelegate(IServiceScopeFactory serviceScopeFactory, ICollection<Type> eventMiddlewareTypes)
        {
            if (_eventDelegate != null)
            {
                using var scope = serviceScopeFactory.CreateScope();
                _eventDelegate = PublishToEventBusAsync;
                foreach (var item in eventMiddlewareTypes.Reverse())
                {
                    _eventDelegate += async (context) =>
                    {
                        await ((IEventMiddleware)scope.ServiceProvider.GetRequiredService(item)).InvokeAsync(context, _eventDelegate);
                    };
                }
            }
        }
    }
}
