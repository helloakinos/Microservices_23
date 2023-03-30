using TradeAnalysisService.Dtos;
using TradeAnalysisService.Models;

namespace TradeAnalysisService.Data;
public interface ITradeAnalysisRepo
{
    bool SaveChanges();

    IEnumerable<Order> GetAllOrders();
    void CreateOrder(Order order);
    bool OrderExists(int orderId);


    IEnumerable<TradeAnalysis> GetTradeAnalysesBasedOnOrder(int orderId);
    TradeAnalysis GetTradeAnalysis(int orderID, int executeId);
    void CreateTradeAnalysis(int orderID, TradeAnalysisCreateDto analysis);
}