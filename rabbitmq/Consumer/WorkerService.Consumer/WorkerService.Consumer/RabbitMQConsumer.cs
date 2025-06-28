using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace WorkerService.Consumer
{
    public class RabbitMQConsumer
    {
        private readonly ILogger<RabbitMQConsumer> _logger;
        private readonly IConfiguration _config;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQConsumer(ILogger<RabbitMQConsumer> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ:Host"],
                Port = int.Parse(_config["RabbitMQ:Port"]),
                UserName = _config["RabbitMQ:User"],
                Password = _config["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

        }

        public void StartConsuming()
        {
            var queue = _config["RabbitMQ:QueueName"];
            _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mensagem = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Mensagem recebida: {mensagem}");

                Console.WriteLine(mensagem);
            };
            
            _channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }

    }
}
