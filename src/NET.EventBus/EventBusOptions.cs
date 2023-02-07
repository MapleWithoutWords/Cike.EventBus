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

        public EventBusOptions()
        {
            Handlers = new List<Type>();
        }

        public void LoadHandlerForAsemmbly(params Assembly[] assemblies)
        {

        }
    }
}
