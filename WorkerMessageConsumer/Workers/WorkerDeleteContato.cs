using Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace WorkerMessageConsumer.Workers
{
    public class WorkerDeleteContato : BackgroundService
    {
        private readonly ILogger<WorkerDeleteContato> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private static string _queueName = "DeleteContato";
        public WorkerDeleteContato(ILogger<WorkerDeleteContato> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory() { HostName = "rabbitQueue" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
            _serviceProvider = serviceProvider; 
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                ProcessMessage(message);
            };
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }
        private void ProcessMessage(string message)
        {
            using var scope = _serviceProvider.CreateAsyncScope();

            var idContato = JsonSerializer.Deserialize<int>(message);
            var contatoCadastro = scope.ServiceProvider.GetRequiredService<IContatoCadastro>();

            contatoCadastro.DeletarContato(idContato);

            Console.WriteLine("Mensagem consumida e gravada: " + message);
        }
    }
}
