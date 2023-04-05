using Cike.EventBus.EventHandlerAbstracts;
using Cike.EventBus.Test.EventData;

namespace Cike.EventBus.Test.EventHandlers
{
    public class UserDeleted2Handler : IDistributedEventHandler<UserDeletedEventArgs>
    {
        private readonly ILogger _logger;

        public UserDeleted2Handler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserDeleted1Handler>();
        }

        public int ExecSeqNo { get; set; } = 1;

        public async Task HandlerAsync(UserDeletedEventArgs eventData)
        {
            Console.WriteLine("UserDeleted2Handler===事件处理程序");
        }
    }
}
