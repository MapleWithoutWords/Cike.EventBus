# Cike.EventBus
#### 介绍

Cike.EventBus 是一个.net core中支持事件总线的一个组件，并且它还支持事件中间件拦截。目前仅支持本地事件，后续会扩展分布式事件。

#### 软件架构

- 本项目依赖于.net6

#### 安装教程

```shell
dotnet add package Cike.EventBus
```

#### 使用说明

##### 功能说明

1. Cike.EventBus 提供了两种事件总线接口它们分别是：

   * ```IDistributedEventBus``` 分布式事件总线 （分布式事件总线，目前默认还是调用本地时间总线，后续会对接分布式队列）

   * ``` ILocalEventBus``` 本地事件总线

   订阅事件处理：允许多次订阅

   * ```IDistributedEventHandler<TEventData>``` 分布式事件处理器
   * ```ILocalEventHandler<TEventData>``` ：本地事件处理器

2. EventMiddleware：事件中间件，允许添加多个中间件，并按照添加顺序执行

   > Cike.EventBus中的事件中间件，允许自定义中间件拦截事件命令，并且可以支持中断事件执行。可以用于一些事件日志的记录，校验等

##### 1. 事件总线使用

1. 在 `Program.cs` 中添加以下代码

```c#
builder.Services.AddCikeEventBus(opt =>
{
    //从某个程序集加载事件处理器
    opt.AddHandlerForAsemmbly(Assembly.Load("xxxx"));
});
```

2. 定义一个事件对象类：```UserDeletedEventArgs```

```c#
public class UserDeletedEventArgs
{
    public int UserId { get; set; }
}
```

3. 在业务代码中注入 IDistributedEventBus 对象（注：目前IDistributedEventBus还是走的本地事件）,并在业务方法中发布事件

```c#
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
```

4. 定义一个事件处理函数：UserDeleted1Handler

```c#
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
```

##### 2. 事件中间件使用

1. 定义一个事件中间件 ```LoggerEventMiddleware```

```c#
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
        
        //一定要加next委托执行，不然将中断事件执行。
        await next(context);
    }
}
```

2. 在添加服务配置的时候添加中间件

```c#
builder.Services.AddCikeEventBus(opt =>
{
    //从某个程序集加载事件处理器
    opt.AddHandlerForAsemmbly(Assembly.Load("xxxx"));
    //添加事件拦截中间件
    opt.UseEventMiddleware<LoggerEventMiddleware>();
});
```

