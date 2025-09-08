using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange
{
    [ApiController]
    [Route("currencies")]
    public class CurrenciesController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public CurrenciesController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public IActionResult GetCurrencies()
        {
            IList<CurrencyDto> currencies = _currencyService.GetAllCurrencies();
            return Ok(currencies);
        }

        [HttpPost]
        public IActionResult InsertCurrency([FromForm] string code, [FromForm] string name, [FromForm] string sign)//валидацию параметров добавишь
        {
            CurrencyDto currency = _currencyService.AddCurrency(name, code, sign);
            return CreatedAtRoute(
                "GetCurrencyByCode",
                new { code = currency.Code },
                currency
            );
        }
    }
}