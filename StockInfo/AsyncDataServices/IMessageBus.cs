using StockInfo.Dtos;

namespace StockInfo.AsyncDataServices;
public interface IMessageBus
{
    void PublishNewStock(ApiRabbitMQPublishDto rabbitMQPublishDto);
}