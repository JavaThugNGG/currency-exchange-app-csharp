namespace CurrencyExchange
{
    public class ExchangeRateService
    {
        private readonly ExchangeRateDao _exchangeRateDao;
        private readonly CurrencyDao _currencyDao;

        public ExchangeRateService(ExchangeRateDao exchangeRateDao, CurrencyDao currencyDao)
        {
            _exchangeRateDao = exchangeRateDao;
            _currencyDao = currencyDao;
        }

        public ExchangeRateDto GetRate(string baseCurrencyCode, string targetCurrencyCode)
        {
            return _exchangeRateDao.GetRate(baseCurrencyCode, targetCurrencyCode);
        }

        public IList<ExchangeRateDto> GetAllRates()
        {
            return _exchangeRateDao.GetAllRates();
        }

        public void AddRate(string baseCurrencyCode, string targetCurrencyCode, decimal rate)
        {
            if (_exchangeRateDao.IsRateExists(baseCurrencyCode, targetCurrencyCode))
            {
                throw new ElementAlreadyExistsException("Данный курс уже существует");
            }

            if (!_currencyDao.IsCurrencyExists(baseCurrencyCode) || !_currencyDao.IsCurrencyExists(targetCurrencyCode))
            {
                throw new ElementNotFoundException("Одна/обе валюты из валютной пары не существуют в бд");
            }
            _exchangeRateDao.InsertRate(baseCurrencyCode, targetCurrencyCode, rate);
        }

        public void UpdateRate(string baseCurrencyCode, string targetCurrencyCode, decimal rate)
        {
            if (_exchangeRateDao.IsRateExists(baseCurrencyCode, targetCurrencyCode))
            {
                _exchangeRateDao.UpdateRate(baseCurrencyCode, targetCurrencyCode, rate);
            }
            else
            {
                throw new ElementNotFoundException("Одна/обе валюты из валютной пары не существуют в бд");
            }
        }
    }
}
