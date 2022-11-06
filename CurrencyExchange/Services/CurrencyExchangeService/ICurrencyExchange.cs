using CurrencyExchange.DTOs;

namespace CurrencyExchange.Services.CurrencyExchangeService;

public interface ICurrencyExchange
{
    Task<GetExchangeRateResponseDto> GetExchangeRate(GetExchangeRateRequestDto request);
}