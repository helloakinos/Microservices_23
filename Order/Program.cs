using Microsoft.EntityFrameworkCore;
using OrderService.AsyncDataServices;
using OrderService.Data;
using OrderService.EventProcessing;
using OrderService.SyncDataServices.Grpc;
using OrderService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ITradeAnalysisDataClient, HttpTradeAnalysisDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddScoped<IApiDataClient, ApiDataClient>();
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddHostedService<MessageBusSubscriber>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));

builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

System.Console.WriteLine($"Command service endpoint is {builder.Configuration["CommandService"]}");

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
app.MapGrpcService<GrpcOrderService>();

app.MapGet("/protos/order.proto", async context =>
    {
        await context.Response.WriteAsync(File.ReadAllText("Protos/orders.proto"));
    });

PrepDb.PrepPopulation(app);

app.Run();
