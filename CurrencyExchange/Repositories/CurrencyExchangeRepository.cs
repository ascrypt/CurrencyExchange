using CurrencyExchange.DTOs;
using CurrencyExchange.Repositories.Entities;

namespace CurrencyExchange.Repositories;

public class CurrencyExchangeRepository : ICurrencyExchangeRepository
{
    private readonly DataContext _dataContext;

    public CurrencyExchangeRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public  void CreateTrade(ExchangeByLatestRateRequestDto exchangeByLatestRateDto)
    {
        var trade = new Trade()
        {
            Amount = exchangeByLatestRateDto.Amount,
            Symbol = exchangeByLatestRateDto.Symbol,
            CreatedAt = DateTime.UtcNow,
            Base = exchangeByLatestRateDto.Base,
        };
        
       _dataContext.Trades.AddAsync(trade); 
       _dataContext.SaveChangesAsync();
    }
}