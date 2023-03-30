using StockInfo.AsyncDataServices;
using StockInfo.Data;
using StockInfo.SyncDataServices.Grpc;
using StockInfo.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IStockInfoRepository, StockInfoRepository>();
builder.Services.AddHttpClient<IOrderDataClient, OrderDataClient>();
builder.Services.AddSingleton<IMessageBus, MessageBus>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcApiService>();
app.MapGet("/protos/stockinfo.proto", async context =>
    {
        await context.Response.WriteAsync(File.ReadAllText("Protos/stockinfo.proto"));
    });

app.Run();
