using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.EventBus.EventHandlerAbstracts
{
    /// <summary>
    /// 对EventHandler包装一层，用于释放非托管资源
    /// </summary>
    public class DefaultEventHandlerDisposeWrapper : IEventHandlerDisposeWrapper
    {
        public IEventHandler EventHandler { get; }
        private readonly Action _disposeAction;

        public DefaultEventHandlerDisposeWrapper(IEventHandler eventHandler, Action disposeAction = null)
        {
            EventHandler = eventHandler;
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            _disposeAction?.Invoke();
        }
    }
}
