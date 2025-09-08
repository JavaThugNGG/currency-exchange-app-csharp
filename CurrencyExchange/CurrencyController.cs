using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange
{
    [ApiController]
    [Route("currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;
        private readonly CurrencyValidator _currencyValidator;
        private readonly CurrencyProcessor _currencyProcessor;

        public CurrencyController(CurrencyService currencyService, CurrencyValidator currencyValidator, CurrencyProcessor currencyProcessor)
        {
            _currencyService = currencyService;
            _currencyValidator = currencyValidator;
            _currencyProcessor = currencyProcessor;
        }

        [HttpGet("{code}", Name = "GetCurrencyByCode")]//валидацию тут можно сделать, мб уберешь класс-валидатор свой
        public IActionResult GetCurrency(string code)
        {
            _currencyValidator.ValidateCode(code);

            CurrencyDto currency = _currencyService.GetCurrency(code);

            return Ok(currency);
        }
    }
}