using CurrencyExchange.DTOs;
using CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIo;
using Microsoft.Extensions.Options;

namespace CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIO;

public class ExchangeRatesApiIoService : IExchangeRatesApiIoService
{
    private readonly HttpClient _httpClient;
    private readonly IOptionsMonitor<ExchangeRatesApiIoOptions> _exchangeRatesApiIoOptions;

    public ExchangeRatesApiIoService(HttpClient httpClient,
        IOptionsMonitor<ExchangeRatesApiIoOptions> exchangeRatesApiIoOptions)
    {
        _exchangeRatesApiIoOptions = exchangeRatesApiIoOptions;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_exchangeRatesApiIoOptions.CurrentValue.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add(
            "apikey", _exchangeRatesApiIoOptions.CurrentValue.ApiKey);
    }

    public async Task<GetExchangeRateResponseDto> GetExchangeRatesAsync(GetExchangeRateRequestDto request) =>
        await _httpClient.GetFromJsonAsync<GetExchangeRateResponseDto>(
            $"exchangerates_data/latest?symbols={request.Symbols}&base={request.Base}") ??
        throw new InvalidOperationException();
}