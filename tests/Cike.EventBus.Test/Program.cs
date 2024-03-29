using Cike.EventBus.Test.EventMiddlewares;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCikeEventBus(opt =>
{
    //从某个程序集加载事件处理器
    opt.AddHandlerForAsemmbly(Assembly.GetEntryAssembly());

    //添加事件拦截中间件
    opt.UseEventMiddleware<LoggerEventMiddleware>()
        .UseEventMiddleware<UserEventMiddleware>();
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test", Version = "v1", Contact = new OpenApiContact { Name = "Test", } });

    foreach (var item in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xml")) c.IncludeXmlComments(item, true);
    c.DocInclusionPredicate((docName, action) => true);
});
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoApiService(opt =>
{
    opt.CreateConventional(Assembly.GetEntryAssembly(), setting =>
    {
        setting.RootPath = "";
    });
});

var app = builder.Build();

app.UseSwagger().UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test"));
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
