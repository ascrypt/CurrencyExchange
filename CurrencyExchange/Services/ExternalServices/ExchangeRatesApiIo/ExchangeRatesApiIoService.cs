using CurrencyExchange.DTOs;
using CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIo;
using CurrencyExchange.Utils;
using Microsoft.Extensions.Options;

namespace CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIO;

public class ExchangeRatesApiIoService : IExchangeRatesApiIoService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptionsMonitor<ExchangeRatesApiIoOptions> _exchangeRatesApiIoOptions;

    public ExchangeRatesApiIoService(IHttpClientFactory httpClientFactory,
        IOptionsMonitor<ExchangeRatesApiIoOptions> exchangeRatesApiIoOptions)
    {
        _exchangeRatesApiIoOptions = exchangeRatesApiIoOptions;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GetExchangeRateResponseDto> GetExchangeRatesAsync(GetExchangeRateRequestDto request)
    {
        var client = _httpClientFactory.CreateClient("ExchangeRatesApiIoService");
        var generateExchangeRatesApiIoServiceUrl = client.BaseAddress?.AbsoluteUri
            .CombineUrl($"exchangerates_data/latest?symbols={request.Symbols}&base={request.Base}");

        var response = await _httpClientFactory.CreateClient("ExchangeRatesApiIoService")
            .GetFromJsonAsync<GetExchangeRateResponseDto>(generateExchangeRatesApiIoServiceUrl);

        return response ?? throw new InvalidOperationException();
    }
}