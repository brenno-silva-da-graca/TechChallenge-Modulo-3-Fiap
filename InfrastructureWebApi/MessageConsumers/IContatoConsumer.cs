using RabbitMQ.Client.Events;

namespace InfrastructureWebApi.MessageConsumers
{
    public interface IContatoConsumer
    {
        EventHandler<BasicDeliverEventArgs> InserirContato { get; }

        EventHandler<BasicDeliverEventArgs> AtualizarContato { get; }

        EventHandler<BasicDeliverEventArgs> DeletarContato { get; }
    }
}