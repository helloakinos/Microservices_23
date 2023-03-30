using StockInfo.Models;

namespace StockInfo.Data;
public interface IStockInfoRepository
{
    bool SaveChanges();

    Stock GetStockById(int id);

    IEnumerable<Stock> GetAllStocks();

    Task<ApiPublishDto> FetchDataFromApi(string date, bool isPurchase, string transactionAmount);
}