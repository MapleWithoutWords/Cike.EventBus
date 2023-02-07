﻿using Microsoft.Extensions.DependencyInjection;
using NET.EventBus.EventHandlerAbstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.EventBus
{
    public abstract class EventBusBase : IEventBus
    {
        protected IServiceScopeFactory ServiceScopeFactory { get; }
        public EventBusBase(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }
        public async Task PublishAsync(Type eventType, object eventData, bool isReleased = false)
        {
            if (isReleased == false)
            {
                //TODO: unit of work
            }
            await PublishToEventBusAsync(eventType, eventData);
        }
        protected abstract Task PublishToEventBusAsync(Type eventType, object eventData);

        public async Task PublishAsync<TEvent>(TEvent eventData, bool isReleased = false)
        {
            await PublishAsync(typeof(TEvent), eventData, isReleased);

        }

        public IDisposable Subscribe<TEventData, TEventHandler>()
                                 where TEventData : class
                                 where TEventHandler : class, IEventHandler, new()
        {
            return Subscribe(typeof(TEventData), new InstanceEventHandlerFactory(new TEventHandler()));
        }

        public IDisposable Subscribe(Type eventDataType, IEventHandler eventHandler)
        {
            return Subscribe(eventDataType, new InstanceEventHandlerFactory(eventHandler));
        }

        public abstract IDisposable Subscribe(Type eventDataType, IEventHandlerFactory eventHandlerFactory);

        public void UnSubscribe(Type eventDataType, IEventHandler eventHandler)
        {
            UnSubscribe(eventDataType, new InstanceEventHandlerFactory(eventHandler));
        }

        public abstract void UnSubscribeAll(Type eventDataType);

        public IDisposable Subscribe<TEventData>(IEventHandler eventHandler)
        {
            return Subscribe(typeof(TEventData), eventHandler);
        }

        public IDisposable Subscribe<TEventData>(IEventHandlerFactory eventHandlerFactory)
        {
            return Subscribe(typeof(TEventData), eventHandlerFactory);
        }

        public void UnSubscribe<TEventData>(IEventHandler eventHandler)
        {
            UnSubscribe(typeof(TEventData), eventHandler);
        }

        public abstract void UnSubscribe(Type eventDataType, IEventHandlerFactory eventHandler);

        public void UnSubscribe<TEventData>(IEventHandlerFactory eventHandlerFactory)
        {
            UnSubscribe(typeof(TEventData), eventHandlerFactory);
        }
    }
}