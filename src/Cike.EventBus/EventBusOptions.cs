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
    public class EventBusOptions
    {
        public List<Type> Handlers { get; protected set; }
        public List<Type> EventMiddlewares { get; protected set; }

        public EventBusOptions()
        {
            Handlers = new List<Type>();
            EventMiddlewares = new List<Type>();
        }

        public EventBusOptions AddHandlerForAsemmbly(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length < 1)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            foreach (var item in assemblies)
            {
                foreach (var typeItem in item.GetTypes())
                {
                    if (typeof(IEventHandler).IsAssignableFrom(typeItem) && typeItem.IsClass)
                    {
                        Handlers.Add(typeItem);
                    }
                }
            }
            return this;
        }

        public EventBusOptions UseEventMiddleware<T>() where T : class, IEventMiddleware
        {
            EventMiddlewares.Add(typeof(T));
            return this;
        }
    }
}
