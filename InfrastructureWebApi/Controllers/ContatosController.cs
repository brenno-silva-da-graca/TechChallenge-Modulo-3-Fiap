using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace InfrastructureWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController : ControllerBase
    {
        private readonly IContatoCadastro _contatoCadastro;
        private readonly IConnectionFactory _rabbitConnectionFactory;
        private readonly ILogger _logger;   

        public ContatosController(IContatoCadastro contatoCadastro, ILoggerFactory loggerFactory)
        {
            _contatoCadastro = contatoCadastro;
            _logger = loggerFactory.CreateLogger(nameof(ContatosController));
            _rabbitConnectionFactory = new ConnectionFactory { HostName = "rabbitQueue" };

            SetupPostContato();
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

            consumer.Received += async (model, ea) =>
            {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var dadosContato = JsonSerializer.Deserialize<Contato>(message);

                    _contatoCadastro.CriarContato(dadosContato, out var _);

                    _logger.LogInformation($"Contato adicionado {dadosContato.DDDID}");
            };

            channel.BasicConsume(queue: "PostContato",
                                 autoAck: true,
                                 consumer: consumer);
        }

        [HttpPut("Atualizar")]
        public IActionResult PutContato([FromBody] Contato dadosContato, int Id)
        {
            dadosContato.Id = Id;
            _contatoCadastro.AtualizarContato(dadosContato);
            return Ok();
        }
        [HttpDelete("Deletar")]
        public IActionResult DeleteContato(int Id)
        {
            _contatoCadastro.DeletarContato(Id);
            return Ok();
        }
    }
}
