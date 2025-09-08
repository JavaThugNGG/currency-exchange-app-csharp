using System.Text.RegularExpressions;

namespace CurrencyExchange.Validators
{
    public class ExchangeValidator
    {
        public void ValidateAmount(string amount)
        {
            if (string.IsNullOrEmpty(amount) || !Regex.IsMatch(amount, @"^\d{1,14}(\.\d{1,14})?$"))
            {
                throw new ArgumentException("Некорректное значение поля amount");
            }
        }
    }
}
