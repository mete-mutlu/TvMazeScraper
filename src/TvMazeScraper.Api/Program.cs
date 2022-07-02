using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using TvMazeScraper.Api;
using TvMazeScraper.Api.AutoMapper;
using TvMazeScraper.Api.BackgroundServices;
using TvMazeScraper.Core;
using TvMazeScraper.Core.Repositories;
using TvMazeScraper.Infrastructure.EntityFramework;
using TvMazeScraper.Infrastructure.EntityFramework.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ScraperDbContext>(opt => opt.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddHttpClient("TvMazeApiClient", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("TvMazeApiUrl"));
}).AddHttpMessageHandler(() =>
        new RateLimitHttpMessageHandler(
            limitCount: 20,
            limitTime: TimeSpan.FromSeconds(10)))
    .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy())
/*.AddPolicyHandler(Policy.RateLimit(20, TimeSpan.FromSeconds(1)).)*/;

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IShowService, ShowService>();

builder.Services.AddHostedService<SchedulerService>();
builder.Services.AddScoped<IScraperBackgroundService, ScraperBackgroundService>();

builder.Services
           .AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();


var app = builder.Build();
app.MapControllers();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();




app.Run();


static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(5, retryAttempt)));
}
static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(5));
}



internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }