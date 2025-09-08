namespace CurrencyExchange.Dto
{
    public record RawExchangeDto
    (
        CurrencyDto BaseCurrency,
        CurrencyDto TargetCurrency,
        decimal Rate,
        decimal Amount
    );
}
