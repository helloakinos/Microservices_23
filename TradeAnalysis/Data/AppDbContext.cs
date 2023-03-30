using TradeAnalysisService.Models;
using Microsoft.EntityFrameworkCore;

namespace TradeAnalysisService.Data;
public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
        
    }
    public DbSet<Order>? Orders { get; set; }
    public DbSet<TradeAnalysis>? TradeAnalyses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Order>()
            .HasMany(p => p.TradeAnalyses)
            .WithOne(p=>p.Order!)
            .HasForeignKey(p=>p.OrderId);

        modelBuilder
            .Entity<TradeAnalysis>()
            .HasOne(p=>p.Order)
            .WithMany(p=>p.TradeAnalyses)
            .HasForeignKey(p=>p.OrderId);
    }
}