namespace CurrencyExchange.DTOs;

    public class GetExchangeRateRequestDto
    {
        public string Base { get; set; }
        public string Symbols { get; set; }
    }
    