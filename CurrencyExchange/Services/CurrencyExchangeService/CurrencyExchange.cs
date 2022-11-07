using CurrencyExchange.DTOs;
using CurrencyExchange.Repositories;
using CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIO;

namespace CurrencyExchange.Services.CurrencyExchangeService;

public class CurrencyExchange : ICurrencyExchange
{
    private readonly IExchangeRatesApiIoService _exchangeRatesApiIoService;
    private readonly ICurrencyExchangeRepository _currencyExchangeRepository;
    
    public CurrencyExchange(IExchangeRatesApiIoService exchangeRatesApiIoService, ICurrencyExchangeRepository currencyExchangeRepository)
    {
        _exchangeRatesApiIoService = exchangeRatesApiIoService;
        _currencyExchangeRepository = currencyExchangeRepository;
    }

    public async Task<GetExchangeRateResponseDto> GetExchangeRate(GetExchangeRateRequestDto request)
    {
        return await _exchangeRatesApiIoService.GetExchangeRatesAsync(request);
    }

    public void ExchangeByLatestRate(ExchangeByLatestRateRequestDto request)
    {
        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than 0");
        }
        _currencyExchangeRepository.CreateTrade(request);
    }
}