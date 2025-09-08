using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange
{
    [ApiController]
    [Route("exchange")]
    public class ExchangeController : ControllerBase
    {
        private readonly ExchangeService _exchangeService;
        private readonly ExchangeValidator _exchangeValidator;
        private readonly ExchangeProcessor _exchangeProcessor;

        public ExchangeController(ExchangeService exchangeService, ExchangeValidator exchangeValidator, ExchangeProcessor exchangeProcessor)
        {
            _exchangeService = exchangeService;
            _exchangeValidator = exchangeValidator;
            _exchangeProcessor = exchangeProcessor;
        }

        [HttpGet]
        public IActionResult Exchange([FromQuery] string from, [FromQuery] string to, [FromQuery] string amount)
        {
            _exchangeProcessor.IsSameCurrencies(from, to);
            _exchangeValidator.ValidateAmount(amount);

            decimal amountDecimal = decimal.Parse(amount, System.Globalization.CultureInfo.InvariantCulture);

            ExchangeDto exchangeDto = _exchangeService.Exchange(from, to, amountDecimal);

            return Ok(exchangeDto);
        }
    }
}