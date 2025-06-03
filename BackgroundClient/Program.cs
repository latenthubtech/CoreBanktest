using System;
using BackgroundClient.Database;
using BackgroundClient.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(config => config
                                            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                            .UseSimpleAssemblyNameTypeSerializer()
                                            .UseRecommendedSerializerSettings()
                                            .UseSqlServerStorage("Server=localhost;Database=CoreBanker;Trusted_Connection=True;Encrypt=False;"));
builder.Services.AddHangfireServer();
var app = builder.Build();

app.UseRouting();

//app.UseEndpoints(endpoints =>
//{
//    // Display Hangfire Dashboard at /hangfire
//    endpoints.MapHangfireDashboard("/hangfire");
//});


app.UseHangfireDashboard("/hangfire", new DashboardOptions { });

app.MapHangfireDashboard();

RecurringJob.AddOrUpdate("job1", () => Console.WriteLine("This job runs every minute!"), Cron.Minutely);

app.Run();
