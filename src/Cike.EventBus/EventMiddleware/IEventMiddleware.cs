using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cike.EventBus.EventMiddleware
{
    public interface IEventMiddleware
    {
        public Task InvokeAsync(EventMiddlewareContext context, EventMiddlewareDelegate next);
    }
}
