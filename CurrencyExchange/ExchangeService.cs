namespace CurrencyExchange
{
    public class ExchangeService
    {
        private static readonly string IntermediateCurrencyCode = "USD";

        private readonly ExchangeProcessor _exchangeProcessor;
        private readonly ExchangeRateDao _exchangeRateDao;
        private readonly ExchangeDao _exchangeDao;

        public ExchangeService(ExchangeProcessor exchangeProcessor, ExchangeRateDao exchangeRateDao, ExchangeDao exchangeDao)//закинуть в di
        {
            _exchangeProcessor = exchangeProcessor;
            _exchangeRateDao = exchangeRateDao;
            _exchangeDao = exchangeDao;
        }

        public ExchangeDto Exchange(string baseCurrencyCode, string targetCurrencyCode, decimal amount)
        {
            if (IsRateExists(baseCurrencyCode, targetCurrencyCode))
            {
                RawExchangeDto rawExchangeDto = _exchangeDao.GetRate(baseCurrencyCode, targetCurrencyCode, amount);
                return _exchangeProcessor.ConvertAmountFromStraightRate(rawExchangeDto, amount);
            }

            if (IsReversedRateExists(baseCurrencyCode, targetCurrencyCode))
            {
                RawExchangeDto rawExchangeDto = _exchangeDao.GetRate(targetCurrencyCode, baseCurrencyCode, amount);
                return _exchangeProcessor.ConvertAmountFromReversedRate(rawExchangeDto, amount);
            }

            if (IsCrossRateExists(baseCurrencyCode, targetCurrencyCode))
            {
                RawExchangeDto baseRateDto = _exchangeDao.GetRate(IntermediateCurrencyCode, baseCurrencyCode, amount);
                RawExchangeDto targetRateDto = _exchangeDao.GetRate(IntermediateCurrencyCode, targetCurrencyCode, amount);

                decimal baseRate = baseRateDto.Rate;
                decimal targetRate = targetRateDto.Rate;

                CurrencyDto baseCurrency = baseRateDto.BaseCurrency;
                CurrencyDto targetCurrency = targetRateDto.TargetCurrency;

                return _exchangeProcessor.ConvertRateFromCrossRate(baseCurrency, targetCurrency, baseRate, targetRate, amount);
            }

            throw new ElementNotFoundException("Запрашиваемая валюта не найдена");
        }

        public bool IsRateExists(string baseCurrencyCode, string targetCurrencyCode)
        {
            return _exchangeRateDao.IsRateExists(baseCurrencyCode, targetCurrencyCode);
        }

        public bool IsReversedRateExists(string baseCurrencyCode, string targetCurrencyCode)
        {
            return _exchangeRateDao.IsRateExists(targetCurrencyCode, baseCurrencyCode);
        }

        public bool IsCrossRateExists(string baseCurrencyCode, string targetCurrencyCode)
        {
            return _exchangeRateDao.IsRateExists(IntermediateCurrencyCode, baseCurrencyCode) && _exchangeRateDao.IsRateExists(IntermediateCurrencyCode, targetCurrencyCode);
        }
    }
}
