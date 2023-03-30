using OrderService.Dtos;

namespace OrderService.AsyncDataServices;
public interface IMessageBusClient
{
    void PublishNewOrder(OrderPublishDto orderPublishDto);
}