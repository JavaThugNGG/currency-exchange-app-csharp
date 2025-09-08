using CurrencyExchange.Dto;

namespace CurrencyExchange.Mappers
{
    public class ExchangeMapper
    {
        public ExchangeDto ToDto(RawExchangeDto rawExchangeDto, decimal rate, decimal amount, decimal convertedAmount)
        {
            CurrencyDto baseCurrency = rawExchangeDto.BaseCurrency;
            CurrencyDto targetCurrency = rawExchangeDto.TargetCurrency;

            return new ExchangeDto(baseCurrency, targetCurrency, rate, amount, convertedAmount);
        }
    }
}
