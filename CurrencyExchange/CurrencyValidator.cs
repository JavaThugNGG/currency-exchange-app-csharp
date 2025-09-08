using System.Text.RegularExpressions;

namespace CurrencyExchange
{
    public class CurrencyValidator
    {
        public void ValidateParameters(string code, string name, string sign)
        {
            ValidateCode(code);
            ValidateName(name);
            ValidateSign(sign);
        }

        public void ValidateCode(string code)
        {
            if (code == null || !Regex.IsMatch(code, @"^[A-Za-z]{1,3}$"))
            {
                throw new ArgumentException("Некорректный аргумент code при добавлении валюты");
            }
        }

        private void ValidateName(string name)
        {
            if (name == null || !Regex.IsMatch(name, "([A-Za-zА]{1,8})( [A-Za-zА]{1,10}){0,2}"))
            {
                throw new ArgumentException("Некорректный аргумент name при добавлении валюты");
            }
        }

        private void ValidateSign(string sign)
        {
            if (sign == null || !Regex.IsMatch(sign, "\\S"))
            {
                throw new ArgumentException("Некорректный аргумент sign при добавлении валюты");
            }
        }
    }
}
