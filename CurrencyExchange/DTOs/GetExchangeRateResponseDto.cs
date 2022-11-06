namespace CurrencyExchange.DTOs;

public class GetExchangeRateResponseDto
{
    public string Base { get; set; }
    public string Date { get; set; }
    public Dictionary<string, double> Rates { get; set; }
    public bool Success { get; set; }
    public int Timestamp { get; set; }
}