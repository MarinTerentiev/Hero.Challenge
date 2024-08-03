using Domain.Common;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Application.RabbitmqPublisher;

public abstract class BaseRabbitmqPublisher<T>
{
    private readonly string _queueName;
    private readonly string _exchangeName;
    private readonly string _rountingName;
    private readonly RabbitmqSettings _rabbitmqSettings;
    private IConnection? _connection;
    private IModel? _channel;

    public BaseRabbitmqPublisher(IOptions<RabbitmqSettings> options, string queueName)
    {
        _rabbitmqSettings = options.Value;
        _queueName = queueName;
        _exchangeName = $"{queueName}Exchange";
        _rountingName = $"{queueName}Rout";

        InitReceiveService();
    }

    private void InitReceiveService()
    {
        ConnectionFactory factory = new();
        factory.Uri = new Uri(_rabbitmqSettings.ConnectionStrings);
        factory.ClientProvidedName = $"App Hero: {_queueName} Publisher";

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
        _channel?.QueueDeclare(_queueName, false, false, false, null);
        _channel?.QueueBind(_queueName, _exchangeName, _rountingName, null);
    }

    public async Task Publishe(T message)
    {
        var body = SerializeMessage(message);

        await Task.Run(() => _channel.BasicPublish(exchange: _exchangeName, routingKey: _rountingName, basicProperties: null, body));
    }

    protected abstract byte[] SerializeMessage(T message);

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
