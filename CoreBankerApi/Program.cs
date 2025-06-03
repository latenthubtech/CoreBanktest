using System.Text.Json.Serialization;
using CoreBankerApi.Application.Services;
using CoreBankerApi.Application.Services.Impl;
using CoreBankerApi.Application.Util;
using CoreBankerApi.Domain.Repository;
using CoreBankerApi.Infrastructure.Database;
using CoreBankerApi.Infrastructure.Queue;
using CoreBankerApi.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register MediatR services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Configure database context
builder.Services.AddDbContextFactory<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoreBankerApiConnection")));

// Register domain services
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // Register the handler

// Add Basic Auth services to the container.
builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", options => { });


// Register repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Configure SQL Server connection
builder.Services.AddDbContext<TaskQueueContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoreBankerApiConnection")));



// Register TaskQueueService
builder.Services.AddScoped<TaskQueueService>();
//Register Background Task Processor
//builder.Services.AddHostedService<QueuedHostedService>();
//builder.Services.AddHostedService<TaskProcessingService>();



var app = builder.Build();

app.UseExceptionHandler( _ => {  }); // Enable exception handling

// API Endpoint to enqueue tasks
//app.MapPost("/enqueue", async (TaskQueueService queueService) =>
//{
//    //throw new NotImplementedException("Not Implemented well");
//    //throw new AppException(ResponseCode.InvalidInput);
//    //string taskMessage = $"Task at {DateTime.UtcNow}";
//    //await queueService.EnqueueTaskAsync(taskMessage, 4);
//    //return Results.Ok($"Task '{taskMessage}' added to queue!");
//});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Apply migrations at startup
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
// 'BVMP'
// L

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
