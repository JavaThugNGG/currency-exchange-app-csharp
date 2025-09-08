namespace CurrencyExchange.Dto
{
    public record ExchangeDto
    (
        CurrencyDto BaseCurrency,
        CurrencyDto TargetCurrency,
        decimal Rate,
        decimal Amount,
        decimal ConvertedAmount
    );
}
