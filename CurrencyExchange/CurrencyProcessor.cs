namespace CurrencyExchange
{
    public class CurrencyProcessor
    {
        public string GetCurrencyCodeWithoutSlash(string currencyCode)
        {
            return currencyCode.Substring(1);
        }
    }
}
