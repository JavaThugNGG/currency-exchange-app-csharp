namespace CurrencyExchange
{
    public class CurrencyService
    {
        private readonly CurrencyDao _currencyDao;

        public CurrencyService(CurrencyDao currencyDao)
        {
            _currencyDao = currencyDao;
        }

        public CurrencyDto GetCurrency(string code)
        {
            return _currencyDao.GetCurrency(code);
        }

        /*internal IList<CurrencyDto> GetAllCurrencies()
        {
            return _currencyDao.GetAllCurrencies();
        }

        internal CurrencyDto AddCurrency(string fullName, string code, string sign)
        {
            if (_currencyDao.IsCurrencyExists(code))
            {
                throw new ElementAlreadyExistsException("Данная валюта уже существует");
            }

            long id = _currencyDao.InsertCurrency(fullName, code, sign);
            return new CurrencyDto(id, fullName, code, sign);
        }*/
    }
}
