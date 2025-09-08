using System.Globalization;
using System.Text;

namespace CurrencyExchange
{
    public class ExchangeRateProcessor
    {
        public string SplitBaseCurrency(string path)
        {
            if (string.IsNullOrEmpty(path) || path.Length < 3)
                throw new ArgumentException("Некорректный путь");
            return path.Substring(0, 3);
        }

        public string SplitTargetCurrency(string path)
        {
            if (string.IsNullOrEmpty(path) || path.Length < 6)
                throw new ArgumentException("Некорректный путь");
            return path.Substring(3, 3);
        }

        public decimal NormalizeRate(string rate)
        {
            if (string.IsNullOrEmpty(rate))
                throw new ArgumentException("Параметр rate некорректный");

            string rateWithDot = rate.Replace(",", ".");
            if (!decimal.TryParse(rateWithDot, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal result))
            {
                throw new ArgumentException("Параметр rate некорректный");
            }

            return result;
        }

        public string ParseRateForPatch(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            string requestBody = reader.ReadToEnd();

            string rateString = null;
            var parameters = requestBody.Split('&', StringSplitOptions.RemoveEmptyEntries);
            foreach (var param in parameters)
            {
                var keyValue = param.Split('=', 2);
                if (keyValue.Length == 2 && keyValue[0] == "rate")
                {
                    rateString = keyValue[1];
                    break;
                }
            }

            return rateString;
        }
    }
}
