using Cike.EventBus.EventHandlerAbstracts;
using Cike.EventBus.Test.EventData;

namespace Cike.EventBus.Test.EventHandlers
{
    public class UserDeleted1Handler : IDistributedEventHandler<UserDeletedEventArgs>
    {
        private readonly ILogger _logger;

        public UserDeleted1Handler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserDeleted1Handler>();
        }

        public async Task HandlerAsync(UserDeletedEventArgs eventData)
        {
            Console.WriteLine("UserDeleted1Handler===事件处理程序");
        }
    }
}
