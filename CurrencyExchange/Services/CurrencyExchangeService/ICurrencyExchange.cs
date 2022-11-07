using CurrencyExchange.DTOs;

namespace CurrencyExchange.Services.CurrencyExchangeService;

public interface ICurrencyExchange
{
    Task<GetExchangeRateResponseDto> GetExchangeRate(GetExchangeRateRequestDto request);
    void ExchangeByLatestRate(ExchangeByLatestRateRequestDto request);
}