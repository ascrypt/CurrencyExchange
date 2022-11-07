using CurrencyExchange.DTOs;

namespace CurrencyExchange.Repositories;

public interface ICurrencyExchangeRepository
{
    void CreateTrade(ExchangeByLatestRateRequestDto exchangeByLatestRateDto);
}