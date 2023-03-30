using StockInfo.Models;
using Microsoft.EntityFrameworkCore;

namespace StockInfo.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
        
    }
    public DbSet<Stock> Stocks { get; set; }
}