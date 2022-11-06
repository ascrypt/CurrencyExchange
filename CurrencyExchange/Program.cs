using CurrencyExchange.Services.CurrencyExchangeService;
using CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIo;
using CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

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
builder.Services.AddHttpClient<ExchangeRatesApiIoService>();

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