using Domain.Common;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receiver.Consumers;

public abstract class BaseRabbitmqConsumer : BackgroundService
{
    private readonly string _queueName;
    private readonly string _exchangeName;
    private readonly string _rountingName;
    private readonly RabbitmqSettings _rabbitmqSettings;
    private IConnection? _connection;
    private IModel? _channel;
    private readonly ILogger<BaseRabbitmqConsumer> _logger;

    public BaseRabbitmqConsumer(IOptions<RabbitmqSettings> options, string queueName, ILogger<BaseRabbitmqConsumer> logger)
    {
        _rabbitmqSettings = options.Value;
        _queueName = queueName;
        _exchangeName = $"{queueName}Exchange";
        _rountingName = $"{queueName}Rout";

        InitConsumerService();
        _logger = logger;
    }

    private void InitConsumerService()
    {
        ConnectionFactory factory = new();
        factory.Uri = new Uri(_rabbitmqSettings.ConnectionStrings);
        factory.ClientProvidedName = $"App Hero: {_queueName} Consumer";

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel?.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
        _channel?.QueueDeclare(_queueName, false, false, false, null);
        _channel?.QueueBind(_queueName, _exchangeName, _rountingName, null);
        _channel?.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, args) =>
        {
            bool processedSuccessfully = false;
            try
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(" [x] Consumer {0}", message);

                processedSuccessfully = await HandleMessageAsync(message, stoppingToken); ;
            }
            catch (Exception ex)
            {
                var error = $"Exception occurred while processing message from queue {_queueName}: {ex}";
                _logger.LogError(ex, error);
            }

            if (processedSuccessfully)
            {
                _channel?.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
            }
            else
            {
                _channel?.BasicReject(deliveryTag: args.DeliveryTag, requeue: true);
            }
        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        return Task.CompletedTask;
    }

    protected abstract Task<bool> HandleMessageAsync(string message, CancellationToken stoppingToken);

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
