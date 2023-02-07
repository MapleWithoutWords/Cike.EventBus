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
    public interface IEventHandlerDisposeWrapper : IDisposable
    {
        public IEventHandler EventHandler { get;}
    }
}
