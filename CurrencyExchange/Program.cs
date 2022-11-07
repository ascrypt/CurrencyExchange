using CurrencyExchange.Repositories;
using CurrencyExchange.Services.CurrencyExchangeService;
using CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIo;
using CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIO;
using CurrencyExchange.Utils.Resiliency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
    config.ApiVersionReader = new HeaderApiVersionReader("api-version");
});
builder.Services.Configure<ExchangeRatesApiIoOptions>(options =>
    configuration.GetSection("Services:ExchangeRatesApiIOSettings").Bind(options));
builder.Services.AddTransient<ICurrencyExchange, CurrencyExchange.Services.CurrencyExchangeService.CurrencyExchange>();
builder.Services.AddTransient<IExchangeRatesApiIoService, ExchangeRatesApiIoService>();
builder.Services.AddTransient<ICurrencyExchangeRepository, CurrencyExchangeRepository>();

builder.Services.AddHttpClient<ExchangeRatesApiIoService>("ExchangeRatesApiIoService", hc =>
    {
        hc.BaseAddress = new Uri(configuration.GetSection("Services")
            .GetValue<string>("ExchangeRatesApiIOSettings:BaseUrl"));
        hc.DefaultRequestHeaders.Add(
            "apikey", configuration.GetSection("Services")["ExchangeRatesApiIOSettings:ApiKey"]);
        hc.Timeout =
            TimeSpan.FromSeconds(
                Convert.ToInt32(
                    configuration.GetSection("ResiliencyPolicies")["Default:OptimisticTimeout"]));
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(
        Convert.ToDouble(configuration.GetSection("ResiliencyPolicies")["Default:LifeTime"])))
    .AddPolicyHandler(ResiliencyPolicies.GetDefaultRetryPolicy(configuration))
    .AddPolicyHandler(ResiliencyPolicies.GetDefaultCircuitBreakerPolicy(configuration));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "RedisDB_";
});
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<DataContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("TradeDbConnection")));


var app = builder.Build();
using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<DataContext>())
{
    context?.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();