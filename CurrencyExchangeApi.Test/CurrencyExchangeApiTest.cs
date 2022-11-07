using System.Net;
using System.Net.Http.Json;
using CurrencyExchange.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CurrencyExchangeApi.Test;

public class CurrencyExchangeApiTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestGetCurrencyExchangeStatusCodeOk()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        // Act 
        var responseMessage = await client.GetAsync("/api/v1/currency-exchange/latest?base=USD&symbols=EUR");
        // Assert
        Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task TestGetCurrencyExchangeMultipleSymbols()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        // Act 
        var getExchangeRateResponseDto =
            await client.GetFromJsonAsync<GetExchangeRateResponseDto>(
                "/api/v1/currency-exchange/latest?base=USD&symbols=EUR,GBP");
        // Assert
        Assert.That(getExchangeRateResponseDto.Rates.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task TestGetCurrencyExchangeSuccess()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        // Act 
        var getExchangeRateResponseDto =
            await client.GetFromJsonAsync<GetExchangeRateResponseDto>(
                "/api/v1/currency-exchange/latest?base=USD&symbols=GBP");
        // Assert
        Assert.That(getExchangeRateResponseDto.Success, Is.EqualTo(true));
    }

    [Test]
    public async Task TestGetCurrencyExchangeStatusCodeNotFound()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        // Act 
        var responseMessage = await client.GetAsync("/api/currency-exchange/latest?base=USD&symbols=EUR");
        // Assert
        Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestGetCurrencyExchangeCurrencyUnknown()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        // Act 
        var responseMessage = await client.GetAsync("/api/v1/currency-exchange/latest?base=TTT&symbols=EUR");
        // Assert
        Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }
    
    [Test]
    public async Task TestExchangeByLatestRateSuccess()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        
        // Act 
        var data = new { Base = "EUR", Amount = 42, Symbol = "USD"};
        var response = await client.PostAsJsonAsync("/api/v1/currency-exchange/latest", data);
        
        // Assert
        Assert.That(response.ReasonPhrase, Is.EqualTo("No Content"));
    }
    
    [Test]
    public async Task TestExchangeByLatestRateInvalidData()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        
        // Act 
        var data = new { Base = "EUR", Amount = 0, Symbol = "USD"};
        var response = await client.PostAsJsonAsync("/api/v1/currency-exchange/latest", data);
        
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}