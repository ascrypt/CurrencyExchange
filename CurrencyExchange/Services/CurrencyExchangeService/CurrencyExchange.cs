using CurrencyExchange.DTOs;
using CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIO;

namespace CurrencyExchange.Services.CurrencyExchangeService;

public class CurrencyExchange : ICurrencyExchange
{
    private readonly IExchangeRatesApiIoService _exchangeRatesApiIoService;

    public CurrencyExchange(IExchangeRatesApiIoService exchangeRatesApiIoService)
    {
        _exchangeRatesApiIoService = exchangeRatesApiIoService;
    }

    public async Task<GetExchangeRateResponseDto> GetExchangeRate(GetExchangeRateRequestDto request)
    {
        return await _exchangeRatesApiIoService.GetExchangeRatesAsync(request);
    }
}