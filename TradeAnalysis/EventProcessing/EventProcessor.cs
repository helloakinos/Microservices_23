using System.Text.Json;
using AutoMapper;
using TradeAnalysisService.Data;
using TradeAnalysisService.Dtos;
using TradeAnalysisService.Models;

namespace TradeAnalysisService.EventProcessing;
public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);
        switch(eventType)
        {
            case EventType.OrderPublished:
                addOrder(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        var eventType =  JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
        switch(eventType.Event)
        {
            case "Order Published":
                return EventType.OrderPublished;
            default:
                return EventType.Undetermined;

        }
    }

    private void addOrder(string orderPublishedMessage)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ITradeAnalysisRepo>();
            var orderPublishedDto = JsonSerializer.Deserialize<OrderPublishDto>(orderPublishedMessage);
            try
            {
                var order = _mapper.Map<Order>(orderPublishedDto);
                if(!repo.OrderExists(order.OrderId))
                {
                    repo.CreateOrder(order);
                    repo.SaveChanges();
                }
                else
                {
                    System.Console.WriteLine($"Order already exists in memory.");
                }
            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"--> Error. Failed to add Order to memory: {ex}");
            }
        }
    }

}
enum EventType
{
    OrderPublished,
    Undetermined
}