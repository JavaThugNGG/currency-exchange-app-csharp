using CurrencyExchange.Dto;
using CurrencyExchange.Services;
using CurrencyExchange.Validators;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers
{
    [ApiController]
    [Route("currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;
        private readonly CurrencyValidator _currencyValidator;

        public CurrencyController(CurrencyService currencyService, CurrencyValidator currencyValidator)
        {
            _currencyService = currencyService;
            _currencyValidator = currencyValidator;
        }

        [HttpGet("{code}", Name = "GetCurrencyByCode")]
        public IActionResult GetCurrency(string code)
        {
            _currencyValidator.ValidateCode(code);
            CurrencyDto currency = _currencyService.GetCurrency(code);
            return Ok(currency);
        }
    }
}