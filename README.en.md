# Cike.EventBus
#### Introduce

'Cike.EventBus' is a component of.NET core that supports the event bus, and it also supports event middleware interception. Currently, only local events are supported. Distributed events will be expanded in the future.

#### Software architecture

- This project relies on.net6

#### Installation tutorial

```shell
dotnet add package Cike.EventBus
```

#### Instructions for use

##### Function description

1. 'Cike.EventBus' provides two event bus interfaces, they are：

   * ```IDistributedEventBus``` ：Distributed event bus (Currently, the distributed event bus still calls the local time bus by default, and will be connected to distributed components later)

   * ``` ILocalEventBus```：Local event bus

   Event subscription handling: Multiple subscriptions are allowed

   * ```IDistributedEventHandler<TEventData>``` Distributed event handler
   * ```ILocalEventHandler<TEventData>``` ：Local event handler

2. EventMiddleware：Event middleware, allowing multiple pieces of middleware to be added in the order they are added

   > Event middleware in 'Cike.EventBus' allows custom middleware to intercept event commands and can support interrupt event execution. Can be used for some event log records, verification and so on

##### 1. Event bus usage

1. Add the following code in 'Program.cs'

```c#
builder.Services.AddCikeEventBus(opt =>
{
    //Loads an event handler from an assembly
    opt.AddHandlerForAsemmbly(Assembly.Load("xxxx"));
});
```

2. Define an event object class：```UserDeletedEventArgs```

```c#
public class UserDeletedEventArgs
{
    public int UserId { get; set; }
}
```

3. Inject the IDistributedEventBus object into the business code (note: for now, the IDistributedEventBus is still a local event) and publish the event in the business method

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
        Console.WriteLine($"userId={userId}user deleted");
        await _distributedEventBus.PublishAsync(new UserDeletedEventArgs
        {
            UserId = userId
        });

    }
}
```

4. Define an event handler: UserDeleted1Handler

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
        Console.WriteLine("UserDeleted1Handler===event handler");
    }
}
```

##### 2. Event middleware usage

1. Define an event middleware： ```LoggerEventMiddleware```

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
        _logger.LogDebug($"event middleware，record log：eventId=【{context.EventId}】、eventType=【{context.EventType}】、event handler count=【{context.EventHandlerFactories.Count}】");
        
        //Be sure to add the next delegate execution, or the event execution will be interrupted.
        await next(context);
    }
}
```

2. Add middleware when adding service configuration

```c#
builder.Services.AddCikeEventBus(opt =>
{
    //load event handler from Assembly
    opt.AddHandlerForAsemmbly(Assembly.Load("xxxx"));
    //add event middleware
    opt.UseEventMiddleware<LoggerEventMiddleware>();
});
```

