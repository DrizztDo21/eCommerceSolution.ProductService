using RabbitMQ.Client;

namespace BusinessLogicLayer.RabbitMQ;
public interface IRabbitMQPublisher
{
    Task PublishAsync<T>(T message, string routingKey);
    void SetChannel(IChannel channel);
}
