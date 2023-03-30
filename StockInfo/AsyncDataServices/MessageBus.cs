using System.Text;
using System.Text.Json;
using StockInfo.Dtos;
using RabbitMQ.Client;

namespace StockInfo.AsyncDataServices;
public class MessageBus : IMessageBus
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;


    public MessageBus(IConfiguration configuration)
    {
        _configuration = configuration;
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"])
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _configuration["RabbitMQExchange"], type: ExchangeType.Fanout);
            
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }
        catch(Exception ex)
        {
            System.Console.WriteLine($"--> Error with the messagebus connection: {ex}");
        }
    }
    public void PublishNewStock(ApiRabbitMQPublishDto rabbitMQPublishDto)
    {
        var message = JsonSerializer.Serialize(rabbitMQPublishDto);
        if (_connection.IsOpen)
        {
            SendMessage(message);
        }
        else
        {
            System.Console.WriteLine("RabbitMQ connection is not open.");
        }
    }
    
    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: _configuration["RabbitMQExchange"],
        routingKey: "",
        basicProperties: null,
        body: body);
    }


    public void Dispose()
    {
        if(_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
            System.Console.WriteLine("RabbitMQ message bus disposed");
        }
    }
    
    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        System.Console.WriteLine("RabbitMQ connection shutdown");
    }
}