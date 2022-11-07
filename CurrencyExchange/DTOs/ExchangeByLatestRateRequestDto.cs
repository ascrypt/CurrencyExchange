namespace CurrencyExchange.DTOs;

    public class ExchangeByLatestRateRequestDto
    {
        public string Base { get; set; }
        public string Symbol { get; set; }
        public double Amount { get; set; }
    }
    