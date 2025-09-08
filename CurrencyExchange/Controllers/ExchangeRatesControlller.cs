using CurrencyExchange.Dto;
using CurrencyExchange.Processors;
using CurrencyExchange.Services;
using CurrencyExchange.Validators;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers
{
    [ApiController]
    [Route("exchangeRates")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ExchangeRateService _exchangeRateService;
        private readonly ExchangeRateValidator _exchangeRateValidator;
        private readonly ExchangeRateProcessor _exchangeRateProcessor;
        private readonly ExchangeProcessor _exchangeProcessor;

        public ExchangeRatesController(ExchangeRateService exchangeRateService, ExchangeRateValidator exchangeRateValidator, ExchangeRateProcessor exchangeRateProcessor, ExchangeProcessor exchangeProcessor)
        {
            _exchangeRateService = exchangeRateService;
            _exchangeRateValidator = exchangeRateValidator;
            _exchangeRateProcessor = exchangeRateProcessor;
            _exchangeProcessor = exchangeProcessor;
        }

        [HttpGet]
        public IActionResult GetExchangeRates()
        {
            IList<ExchangeRateDto> exchangeRates = _exchangeRateService.GetAllRates();
            return Ok(exchangeRates);
        }

        [HttpPost]
        public IActionResult InsertExchangeRate([FromForm] string baseCurrencyCode, [FromForm] string targetCurrencyCode, [FromForm] string rate)
        {
            _exchangeRateValidator.ValidateParameters(baseCurrencyCode, targetCurrencyCode, rate);
            _exchangeProcessor.IsSameCurrencies(baseCurrencyCode, targetCurrencyCode);

            decimal rateDecimal = _exchangeRateProcessor.NormalizeRate(rate);

            _exchangeRateService.AddRate(baseCurrencyCode, targetCurrencyCode, rateDecimal);
            ExchangeRateDto exchangeRate = _exchangeRateService.GetRate(baseCurrencyCode, targetCurrencyCode);

            string pairCode = baseCurrencyCode + targetCurrencyCode;

            return CreatedAtRoute("GetExchangeRate",new { pairCode },exchangeRate);
        }
    }
}