using Cike.EventBus.EventMiddleware;

namespace Cike.EventBus.Test.EventMiddlewares
{
    public class UserEventMiddleware : IEventMiddleware
    {
        private readonly ILogger<UserEventMiddleware> _logger;
        public UserEventMiddleware(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserEventMiddleware>();
        }

        public async Task InvokeAsync(EventMiddlewareContext context, EventMiddlewareDelegate next)
        {
            _logger.LogDebug($"UserEventMiddleware,事件id=【{context.EventId}】");
            await next(context);
        }
    }
}
