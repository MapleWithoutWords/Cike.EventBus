﻿using Cike.EventBus.DistributedEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cike.EventBus.EventHandlerAbstracts
{
    public delegate Task EventHandlerExecuteAsync(IEventHandler target, object parameter);

    public interface IEventHanlderExecute
    {
        public EventHandlerExecuteAsync ExecuteAsync { get; }
    }
    public class LocalEventHanlderExecute<TEventData> : IEventHanlderExecute
    {
        public EventHandlerExecuteAsync ExecuteAsync { get => async (target, parameter) => await((ILocalEventHandler<TEventData>)target).HandlerAsync((TEventData)parameter); }
    }
    public class DistributedEventHanlderExecute<TEventData> : IEventHanlderExecute
    {
        public EventHandlerExecuteAsync ExecuteAsync { get => async (target, parameter) => await((IDistributedEventHandler<TEventData>)target).HandlerAsync((TEventData)parameter); }
    }
}
