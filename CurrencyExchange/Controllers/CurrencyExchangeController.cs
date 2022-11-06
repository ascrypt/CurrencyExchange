using System.Net;
using CurrencyExchange.DTOs;
using CurrencyExchange.Services.CurrencyExchangeService;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/currency-exchange/latest")]
public class CurrencyExchangeController : ControllerBase
{
    private readonly ICurrencyExchange _currencyExchangeService;

    public CurrencyExchangeController(ICurrencyExchange currencyExchange)
    {
        _currencyExchangeService = currencyExchange;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetExchangeRateResponseDto), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<GetExchangeRateResponseDto>> GetLatestExchangeRate(
        [FromQuery] GetExchangeRateRequestDto getExchangeRateRequestDto)
    {
        var exchangeRate = await _currencyExchangeService.GetExchangeRate(getExchangeRateRequestDto);

        return Ok(exchangeRate);
    }
}