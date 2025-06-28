namespace WorkerService.Consumer
{
    public class Worker(ILogger<Worker> logger, RabbitMQConsumer consumer) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            consumer.StartConsuming();

            // Apenas mant�m o worker "vivo"
            return Task.CompletedTask;
        }

    }
}
