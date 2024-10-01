using RabbitMQ.Client;
using Application.Interfaces;
using RabbitMQ.Client.Events;
using Microsoft.AspNetCore.Mvc;
using InfrastructureWebApi.MessageConsumers;

namespace InfrastructureWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController : ControllerBase
    {
        private readonly IContatoCadastro _contatoCadastro;
        private readonly IConnectionFactory _rabbitConnectionFactory;
        private readonly IContatoConsumer _contatoConsumer;
        private readonly ILogger _logger;

        public ContatosController(IContatoCadastro contatoCadastro, ILoggerFactory loggerFactory, IContatoConsumer contatoConsumer)
        {
            _contatoCadastro = contatoCadastro;
            _contatoConsumer = contatoConsumer;
            _logger = loggerFactory.CreateLogger(nameof(ContatosController));
            _rabbitConnectionFactory = new ConnectionFactory { HostName = "rabbitQueue" };

            SetupPostContato();
            SetupPatchContato();
            SetupDeleteContato();
        }

        [HttpGet("Listar")]
        public IActionResult GetContato()
        {
            return Ok(_contatoCadastro.ListarContatos());
        }

        [HttpGet("ListarPorDDD")]
        public IActionResult ListarPorDDD(int NumDDD)
        {
            return Ok(_contatoCadastro.ListarPorDDD(NumDDD));
        }

        private void SetupPostContato()
        {
            using var connection = _rabbitConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "PostContato",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += _contatoConsumer.InserirContato;

            channel.BasicConsume(queue: "PostContato",
                                 autoAck: true,
                                 consumer: consumer);
        }

        private void SetupPatchContato()
        {
            using var connection = _rabbitConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "PatchContato",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += _contatoConsumer.AtualizarContato;

            channel.BasicConsume(queue: "PatchContato",
                                 autoAck: true,
                                 consumer: consumer);
        }

        private void SetupDeleteContato()
        {
            using var connection = _rabbitConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "DeleteContato",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += _contatoConsumer.DeletarContato;

            channel.BasicConsume(queue: "DeleteContato",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
