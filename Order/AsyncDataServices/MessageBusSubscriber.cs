using System.Text;
using OrderService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.AsyncDataServices;
public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;

    public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProcessor)
    {
        _config = config;
        _eventProcessor = eventProcessor;

        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMQHost"],
            Port = int.Parse(_config["RabbitMQPort"])
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: _config["RabbitMQExchange"], type: ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queueName, exchange: _config["RabbitMQExchange"], routingKey: "");
        System.Console.WriteLine("listening on MessageBus");

        _connection.ConnectionShutdown += RabbitMQConnectionShutdow;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (ModuleHandle, ea) =>
        {
            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            _eventProcessor.ProcessEvent(notificationMessage);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    private void RabbitMQConnectionShutdow(object sender, ShutdownEventArgs e)
    {
        System.Console.WriteLine("RabbitMQ connection shutdown");
    }

    public override void Dispose()
    {
        if(_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
        base.Dispose();
        System.Console.WriteLine("RabbitMQ Dispose method called and executed");
    }
}