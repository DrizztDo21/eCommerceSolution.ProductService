using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BusinessLogicLayer.RabbitMQ;
public class RabbitMQPublisher : IRabbitMQPublisher
{
    private IChannel? _channel;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMQPublisher> _logger;

    public RabbitMQPublisher(IConfiguration configuration, ILogger<RabbitMQPublisher> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void SetChannel(IChannel channel)
    {
        _channel = channel;
    }

    public async Task PublishAsync<T>(T message, string routingKey)
    {

        if (_channel == null)
        {
            throw new InvalidOperationException("Channel is not set. Please set the channel before publishing messages.");
        }

        string messageJson = JsonSerializer.Serialize(message);
        byte[] encodedMessage = Encoding.UTF8.GetBytes(messageJson);


        //Create exchange
        string exchangeName = _configuration["RABBITMQ_PRODUCTS_EXCHANGE"]!;

        await _channel.ExchangeDeclareAsync(
            exchange: exchangeName, 
            type: ExchangeType.Direct, 
            durable: true);

        //Publish message
        _logger.LogInformation("Publishing message to exchange {Exchange} with routing key {RoutingKey}: {Message}", exchangeName, routingKey, messageJson);

        await _channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            body: encodedMessage);

    }
}
