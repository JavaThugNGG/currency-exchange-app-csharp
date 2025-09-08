namespace CurrencyExchange
{
    public record ExchangeRateDto(
        long Id,
        CurrencyDto BaseCurrency,
        CurrencyDto TargetCurrency,
        decimal Rate
    );
}
