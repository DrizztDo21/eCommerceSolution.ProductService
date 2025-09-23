using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMQHostedService : IHostedService, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IRabbitMQPublisher _publisher;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMQHostedService(IConfiguration configuration, IRabbitMQPublisher publisher)
    {
        _configuration = configuration;
        _publisher = publisher;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RABBITMQ_HOST"]!,
            Port = int.Parse(_configuration["RABBITMQ_PORT"]!),
            UserName = _configuration["RABBITMQ_USER"]!,
            Password = _configuration["RABBITMQ_PASSWORD"]!
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        _publisher.SetChannel(_channel);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }


}
