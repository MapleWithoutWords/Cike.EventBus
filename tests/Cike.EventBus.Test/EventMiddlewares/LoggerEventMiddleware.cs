using Cike.EventBus.EventMiddleware;

namespace Cike.EventBus.Test.EventMiddlewares
{
    public class LoggerEventMiddleware : IEventMiddleware
    {
        private readonly ILogger _logger;
        public LoggerEventMiddleware(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LoggerEventMiddleware>();
        }
        public async Task InvokeAsync(EventMiddlewareContext context, EventMiddlewareDelegate next)
        {
            _logger.LogDebug($"事件中间件，记录日志：事件id=【{context.EventId}】、事件类型=【{context.EventType}】、事件处理器个数=【{context.EventHandlerFactories.Count}】");

            await next(context);
        }
    }
}
