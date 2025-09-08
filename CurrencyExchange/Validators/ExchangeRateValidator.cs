using System.Globalization;
using System.Text.RegularExpressions;

namespace CurrencyExchange.Validators
{
    public class ExchangeRateValidator
    {
        public void ValidateParameters(string baseCode, string targetCode, string rate)
        {
            if (string.IsNullOrEmpty(baseCode) || string.IsNullOrEmpty(targetCode) ||
                !Regex.IsMatch(baseCode, "^[A-Za-z]{1,3}$") || !Regex.IsMatch(targetCode, "^[A-Za-z]{1,3}$"))
            {
                throw new ArgumentException("Отсутсвуют необходимые парамметры, либо они некорректные");
            }

            ValidateRate(rate);
        }

        public void ValidateRate(string rate)
        {
            if (string.IsNullOrEmpty(rate))
            {
                throw new ArgumentException("Параметр rate некорректный");
            }

            string normalizedRate = rate.Replace(",", ".");

            if (!decimal.TryParse(normalizedRate, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal result) ||
                result <= 0)
            {
                throw new ArgumentException("Параметр rate некорректный");
            }
        }
    }
}
