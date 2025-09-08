using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange
{
    [ApiController]
    [Route("exchangeRate")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ExchangeRateService _exchangeRateService;
        private readonly ExchangeRateValidator _exchangeRateValidator;
        private readonly ExchangeRateProcessor _exchangeRateProcessor;

        public ExchangeRateController(ExchangeRateService exchangeRateService, ExchangeRateValidator exchangeRateValidator, ExchangeRateProcessor exchangeRateProcessor)
        {
            _exchangeRateService = exchangeRateService;
            _exchangeRateValidator = exchangeRateValidator;
            _exchangeRateProcessor = exchangeRateProcessor;
        }

        [HttpGet("{pairCode}", Name = "GetExchangeRate") ]
        public IActionResult GetExchangeRate(string pairCode)
        {
            string baseCurrencyCode = _exchangeRateProcessor.SplitBaseCurrency(pairCode);
            string targetCurrencyCode = _exchangeRateProcessor.SplitTargetCurrency(pairCode);

            ExchangeRateDto exchangeRate = _exchangeRateService.GetRate(baseCurrencyCode, targetCurrencyCode);
            return Ok(exchangeRate);
        }

        [HttpPatch("{pairCode}")]
        public IActionResult UpdateExchangeRate(string pairCode, [FromForm] string rate)
        {

            string baseCurrencyCode = _exchangeRateProcessor.SplitBaseCurrency(pairCode);
            string targetCurrencyCode = _exchangeRateProcessor.SplitTargetCurrency(pairCode);

            _exchangeRateValidator.ValidateRate(rate);

            decimal rateDecimal = _exchangeRateProcessor.NormalizeRate(rate);

            _exchangeRateService.UpdateRate(baseCurrencyCode, targetCurrencyCode, rateDecimal);
            ExchangeRateDto updatedRate = _exchangeRateService.GetRate(baseCurrencyCode, targetCurrencyCode);
            return Ok(updatedRate);
        }
    }
}