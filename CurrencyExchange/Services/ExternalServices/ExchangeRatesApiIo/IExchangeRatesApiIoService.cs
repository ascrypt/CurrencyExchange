using CurrencyExchange.DTOs;

namespace CurrencyExchange.Services.ExternalServices.ExchangeRatesApiIO;

public interface IExchangeRatesApiIoService
{ 
    Task<GetExchangeRateResponseDto> GetExchangeRatesAsync(GetExchangeRateRequestDto request);
}
