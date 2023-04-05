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

        public List<IEventHandlerFactory> EventHandlerFactoies { get; protected set; }

        public List<Type> EventMiddlewares { get; protected set; }

        public EventBusOptions()
        {
            Handlers = new List<Type>();
            EventMiddlewares = new List<Type>();
            EventHandlerFactoies = new List<IEventHandlerFactory>();
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
                    if (typeItem.IsAbstract || typeItem.IsClass == false)
                    {
                        continue;
                    }
                    if (typeof(IEventHandler).IsAssignableFrom(typeItem))
                    {
                        Handlers.Add(typeItem);
                        continue;
                    }
                    //foreach (var methodItem in typeItem.GetMethods())
                    //{
                    //    var eventHanlderAttr = methodItem.GetCustomAttribute<EventHandlerAttribute>();
                    //    if (eventHanlderAttr != null)
                    //    {

                    //    }
                    //}
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
