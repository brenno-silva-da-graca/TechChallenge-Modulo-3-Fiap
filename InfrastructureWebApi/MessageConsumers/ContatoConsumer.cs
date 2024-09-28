using Application.Interfaces;
using Domain.Entities;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InfrastructureWebApi.MessageConsumers
{
    public class ContatoConsumer : IContatoConsumer
    {
        private IServiceProvider ServiceProvider { get; set; }
        private ILogger Logger { get; set; }

        public ContatoConsumer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            ServiceProvider = serviceProvider;
            Logger = loggerFactory.CreateLogger(nameof(ContatoConsumer));
        }

        public EventHandler<BasicDeliverEventArgs> InserirContato => (object? model, BasicDeliverEventArgs ea) =>
        {
            using var scope = ServiceProvider.CreateAsyncScope();

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var dadosContato = JsonSerializer.Deserialize<Contato>(message);
            var contatoCadastro = scope.ServiceProvider.GetRequiredService<IContatoCadastro>();

            contatoCadastro.CriarContato(dadosContato, out var _);

            Logger.LogInformation($"Contato adicionado {dadosContato.DDDID}");
        };

        public EventHandler<BasicDeliverEventArgs> AtualizarContato => (object? model, BasicDeliverEventArgs ea) =>
        {
            using var scope = ServiceProvider.CreateAsyncScope();

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var dadosContato = JsonSerializer.Deserialize<Contato>(message);
            var contatoCadastro = scope.ServiceProvider.GetRequiredService<IContatoCadastro>();

            contatoCadastro.AtualizarContato(dadosContato);

            Logger.LogInformation($"Contato atualizado {dadosContato.DDDID}");
        };

        public EventHandler<BasicDeliverEventArgs> DeletarContato => (object? model, BasicDeliverEventArgs ea) =>
        {
            using var scope = ServiceProvider.CreateAsyncScope();

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var id = JsonSerializer.Deserialize<int>(message);
            var contatoCadastro = scope.ServiceProvider.GetRequiredService<IContatoCadastro>();

            contatoCadastro.DeletarContato(id);

            Logger.LogInformation($"Contato id:[{id}] removido");
        }
    }
}
