using Cike.EventBus;
using Cike.EventBus.DistributedEvent;
using Cike.EventBus.EventMiddleware;
using Cike.EventBus.LocalEvent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {

        public static void AddCikeEventBus(this IServiceCollection services, Action<EventBusOptions> eventBusOptionsAction = null)
        {
            services.Configure<EventBusOptions>(opt =>
            {
                if (eventBusOptionsAction == null)
                {
                    opt.AddHandlerForAsemmbly();
                }
                else
                {
                    eventBusOptionsAction(opt);
                }
            });

            services.AddSingleton<IDistributedEventBus, DistributedEventBus>();
            services.AddSingleton<ILocalEventBus, LocalEventBus>();

            using var serviceProvider = services.BuildServiceProvider();

            var eventBusOpt = serviceProvider.GetRequiredService<IOptions<EventBusOptions>>();

            foreach (var item in eventBusOpt.Value.Handlers)
            {
                services.AddTransient(item);
            }
            foreach (var item in eventBusOpt.Value.EventMiddlewares)
            {
                services.AddSingleton(typeof(IEventMiddleware), item);
            }

        }
    }
}
