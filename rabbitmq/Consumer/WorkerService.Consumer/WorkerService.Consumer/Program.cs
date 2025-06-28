using WorkerService.Consumer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<RabbitMQConsumer>();
        services.AddHostedService<Worker>();
    }).Build();



host.Run();
