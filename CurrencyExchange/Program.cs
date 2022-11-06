using CurrencyExchange.Services.CurrencyExchangeService;
using CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIo;
using CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIO;
using CurrencyExchange.Utils.Resiliency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Polly;

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
builder.Services.AddHttpClient<ExchangeRatesApiIoService>("ExchangeRatesApiIoService", hc =>
    {
        hc.BaseAddress = new Uri(configuration.GetSection("Services").GetValue<string>("ExchangeRatesApiIOSettings:BaseUrl"));
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
var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
    TimeSpan.FromSeconds(10));
var longTimeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
    TimeSpan.FromSeconds(30));

var policyRegistry = builder.Services.AddPolicyRegistry();

policyRegistry.Add("Regular", timeoutPolicy);
policyRegistry.Add("Long", longTimeoutPolicy);

builder.Services.AddHttpClient("PollyRegistryRegular")
    .AddPolicyHandlerFromRegistry("Regular");

builder.Services.AddHttpClient("PollyRegistryLong")
    .AddPolicyHandlerFromRegistry("Long");
var app = builder.Build();

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