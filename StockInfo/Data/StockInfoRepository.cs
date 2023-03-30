using System.Text.Json.Nodes;
using StockInfo.Models;
using AutoMapper;

namespace StockInfo.Data;
public class StockInfoRepository : IStockInfoRepository
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public StockInfoRepository(AppDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public Stock GetStockById(int id)
    {
        return _context.Stocks.FirstOrDefault(s=>s.ApiId == id);
    }

    public bool SaveChanges()
    {
        return (_context.SaveChanges() >= 0);
    }

    public IEnumerable<Stock> GetAllStocks()
    {
        return _context.Stocks.ToList();
    }

    public async Task<ApiPublishDto> FetchDataFromApi(string date, bool isPurchase, string transactionAmount)
        {
            using HttpResponseMessage response = await sharedClient.GetAsync("");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            JsonNode parsedJson = JsonNode.Parse(jsonResponse)!;
            string openingPrice = parsedJson["Time Series (Daily)"]![$"{date}"]!["1. open"]!.GetValue<string>();
            string high = parsedJson["Time Series (Daily)"]![$"{date}"]!["2. high"]!.GetValue<string>();
            string low = parsedJson["Time Series (Daily)"]![$"{date}"]!["3. low"]!.GetValue<string>();
            string close = parsedJson["Time Series (Daily)"]![$"{date}"]!["4. close"]!.GetValue<string>();
            string adjustedClose = parsedJson["Time Series (Daily)"]![$"{date}"]!["5. adjusted close"]!.GetValue<string>();
            string volume = parsedJson["Time Series (Daily)"]![$"{date}"]!["6. volume"]!.GetValue<string>();
            string dividendAmount = parsedJson["Time Series (Daily)"]![$"{date}"]!["7. dividend amount"]!.GetValue<string>();
            string splitCoefficient = parsedJson["Time Series (Daily)"]![$"{date}"]!["8. split coefficient"]!.GetValue<string>();
            
            
            Stock newStock = new();
            
            newStock.AdjustedClose = adjustedClose;
            newStock.Open = openingPrice;
            newStock.SplitCoefficient= splitCoefficient;
            newStock.High = high;
            newStock.Low = low;
            newStock.Close = close;
            newStock.DividendAmount = dividendAmount;
            newStock.Volume = volume;
            newStock.Date = date;
            newStock.IsPurchase = isPurchase;
            newStock.TransactionAmount = transactionAmount;

            if(newStock == null)
            {
              throw new ArgumentNullException(nameof(newStock));
            }
            else
            {
              _context.Stocks.Add(newStock);
              _context.SaveChanges();
            }

              return _mapper.Map<ApiPublishDto>(newStock);
        }        

    
    static string symbol = "IBM";
    static string apiKey = "";
    public HttpClient sharedClient = new()
    {
        BaseAddress = new Uri($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={symbol}&apikey={apiKey}&datatype=json")
    };
}
