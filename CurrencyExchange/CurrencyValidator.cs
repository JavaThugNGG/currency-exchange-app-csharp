using System.Text.RegularExpressions;

namespace CurrencyExchange
{
    public class CurrencyValidator
    {
        public bool ValidateCode(string code)
        {
            return code != null && Regex.IsMatch(code, @"^[A-Za-z]{1,3}$");
        }
    }
}
