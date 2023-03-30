using StockInfo.Models;

namespace StockInfo.SyncDataServices.Http;
public interface IOrderDataClient
{
    Task SendStockDataToOrder(Stock apiData);
}