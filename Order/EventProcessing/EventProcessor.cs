using System.Text.Json;
using AutoMapper;
using OrderService.Data;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.EventProcessing;
public class EventProcessor : IEventProcessor
{
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _scopeFactory;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _mapper = mapper;
        _scopeFactory = scopeFactory;
    }
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.ApiPublished:
                addStock(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string eventNotification)
    {
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(eventNotification);

        switch (eventType.Event)
        {
            case "Api Published":
                return EventType.ApiPublished;

            case "Order Published":
                return EventType.OrderPublished;

            default:
                return EventType.Undefined;
        }
    }

    private void addStock(string publishedMessage)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<IOrderRepo>();
            var publishedDto = JsonSerializer.Deserialize<StockRabbitMQPublishDto>(publishedMessage);
        try
        {
            var stockReport = _mapper.Map<Order>(publishedDto);
            if(!repo.ApiExists(stockReport.ApiId))
            {
                repo.CreateOrder(stockReport);
                repo.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("Stock information already exists in memory.");
            }
        }
        catch(Exception ex)
        {
            System.Console.WriteLine($"--> Error, failed to add stock to database: {ex}");
        }
        }
    }
}

enum EventType
{
    ApiPublished,
    OrderPublished,
    Undefined

}