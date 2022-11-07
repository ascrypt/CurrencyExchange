using System.ComponentModel.DataAnnotations;
using System.Net;
using CurrencyExchange.DTOs;
using CurrencyExchange.Services.CurrencyExchangeService;
using CurrencyExchange.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace CurrencyExchange.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/currency-exchange/latest")]
public class CurrencyExchangeController : ControllerBase
{
    private readonly ICurrencyExchange _currencyExchangeService;
    private IDistributedCache _cache;

    public CurrencyExchangeController(ICurrencyExchange currencyExchange, IDistributedCache cache)
    {
        _currencyExchangeService = currencyExchange;
        _cache = cache;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetExchangeRateResponseDto), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<GetExchangeRateResponseDto>> LatestExchangeRate(
        [FromQuery] GetExchangeRateRequestDto getExchangeRateRequestDto)
    {
        var recordKey = $"ExchangeRate_{getExchangeRateRequestDto.Base}_{getExchangeRateRequestDto.Symbols}_{DateTime.Now:yyyyMMdd_hhmm}";

        var cachedExchangeRate = await _cache.GetRecordAsync<GetExchangeRateResponseDto>(recordKey);

        if (cachedExchangeRate is not null) return Ok(cachedExchangeRate);
        var exchangeRate = await _currencyExchangeService.GetExchangeRate(getExchangeRateRequestDto); 
        await _cache.SetRecordAsync(recordKey, exchangeRate); 
        return Ok(exchangeRate);

    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult ExchangeByLatestRate(
        [FromBody] ExchangeByLatestRateRequestDto exchangeByLatestRateRequestDto)
    {
        try
        {
            _currencyExchangeService.ExchangeByLatestRate(exchangeByLatestRateRequestDto);
        }
        catch (Exception)
        {
            return BadRequest();
        }
            
        return NoContent();
    }
}