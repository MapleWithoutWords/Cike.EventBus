using NET.EventBus.EventHandlerAbstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NET.EventBus
{
    public class EventBusOptions
    {
        public List<Type> Handlers { get; set; }
        public List<Type> EventMiddlewares { get; set; }

        public EventBusOptions()
        {
            Handlers = new List<Type>();
            EventMiddlewares = new List<Type>();
        }

        public void AddHandlerForAsemmbly(params Assembly[] assemblies)
        {

        }

        public void UseEventMiddleware<T>()
        {
            EventMiddlewares.Add(typeof(T));
        }
    }
}
