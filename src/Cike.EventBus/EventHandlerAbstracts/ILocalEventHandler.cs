using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cike.EventBus.EventHandlerAbstracts
{
    public interface ILocalEventHandler<TEventData> : IEventHandler<TEventData>
    {
    }
}
