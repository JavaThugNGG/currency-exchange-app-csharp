using CurrencyExchange.Dto;
using CurrencyExchange.Services;
using CurrencyExchange.Validators;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers
{
    [ApiController]
    [Route("currencies")]
    public class CurrenciesController : ControllerBase
    {
        private readonly CurrencyService _currencyService;
        private readonly CurrencyValidator _currencyValidator;

        public CurrenciesController(CurrencyService currencyService, CurrencyValidator currencyValidator)
        {
            _currencyService = currencyService;
            _currencyValidator = currencyValidator;
        }

        [HttpGet]
        public IActionResult GetCurrencies()
        {
            IList<CurrencyDto> currencies = _currencyService.GetAllCurrencies();
            return Ok(currencies);
        }

        [HttpPost]
        public IActionResult InsertCurrency([FromForm] string code, [FromForm] string name, [FromForm] string sign)
        {
            _currencyValidator.ValidateParameters(code, name, sign);
            CurrencyDto currency = _currencyService.AddCurrency(name, code, sign);

            return CreatedAtRoute("GetCurrencyByCode", new { code = currency.Code }, currency);
        }
    }
}