using CurrencyExchange.Dao;
using CurrencyExchange.Dto;
using CurrencyExchange.Exceptions;

namespace CurrencyExchange.Services
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

        public IList<CurrencyDto> GetAllCurrencies()
        {
            return _currencyDao.GetAllCurrencies();
        }

        public CurrencyDto AddCurrency(string name, string code, string sign)
        {
            if (_currencyDao.IsCurrencyExists(code))
            {
                throw new ElementAlreadyExistsException("Данная валюта уже существует");
            }

            long id = _currencyDao.InsertCurrency(name, code, sign);
            return new CurrencyDto(id, name, code, sign);
        }
    }
}
