using Cike.AutoWebApi.Setting;
using Cike.EventBus.DistributedEvent;
using Cike.EventBus.Test.EventData;

namespace Cike.EventBus.Test.Application
{
    public class UserAppService : IAutoApiService
    {
        private readonly IDistributedEventBus _distributedEventBus;

        public UserAppService(IDistributedEventBus distributedEventBus)
        {
            _distributedEventBus = distributedEventBus;
        }

        public async Task DeleteAsync(int userId)
        {
            Console.WriteLine($"用户id={userId}的用户被删除了");
            await _distributedEventBus.PublishAsync(new UserDeletedEventArgs
            {
                UserId = userId
            });

        }
    }
}
