using CurrencyExchange.Dto;
using CurrencyExchange.Mappers;
using Microsoft.Data.Sqlite;
using CurrencyExchange.Exceptions;

namespace CurrencyExchange.Processors
{
    public class ExchangeProcessor
    {
        private readonly ExchangeMapper _exchangeMapper;
        public ExchangeProcessor(ExchangeMapper exchangeMapper)
        {
            _exchangeMapper = exchangeMapper;
        }

        public ExchangeDto ConvertAmountFromStraightRate(RawExchangeDto rawExchangeDto, decimal amount)
        {
            decimal rate = rawExchangeDto.Rate;
            decimal convertedAmount = rate * amount;
            decimal roundedConvertedAmount = Math.Round(convertedAmount, 2, MidpointRounding.AwayFromZero);
            try
            {
                return _exchangeMapper.ToDto(rawExchangeDto, rate, amount, roundedConvertedAmount);
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException("Ошибка при обращении к БД!", ex);
            }
        }

        public ExchangeDto ConvertAmountFromReversedRate(RawExchangeDto rawExchangeDto, decimal amount)
        {
            decimal rate = rawExchangeDto.Rate;
            decimal reversedRate = Math.Round(1m / rate, 8, MidpointRounding.AwayFromZero);
            decimal convertedAmount = reversedRate * amount;
            decimal roundedConvertedAmount = Math.Round(convertedAmount, 2, MidpointRounding.AwayFromZero);
            try
            {
                return _exchangeMapper.ToDto(rawExchangeDto, rate, amount, roundedConvertedAmount);
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException("Ошибка при обращении к БД!", ex);
            }
        }

        public ExchangeDto ConvertRateFromCrossRate(CurrencyDto baseCurrency, CurrencyDto targetCurrency, decimal baseRate, decimal targetRate, decimal amount)
        {
            decimal inverseBaseRate = Math.Round(1m / baseRate, 8, MidpointRounding.AwayFromZero);
            decimal inverseTargetRate = Math.Round(1m / targetRate, 8, MidpointRounding.AwayFromZero);
            decimal rate = Math.Round(inverseBaseRate / inverseTargetRate, 8, MidpointRounding.AwayFromZero);

            decimal convertedAmount = rate * amount;
            decimal roundedConvertedAmount = Math.Round(convertedAmount, 2, MidpointRounding.AwayFromZero);

            return new ExchangeDto(baseCurrency, targetCurrency, rate, amount, roundedConvertedAmount);
        }

        public void IsSameCurrencies(string from, string to)
        {
            if (from == to)
            {
                throw new ArgumentException("Валютная пара должна состоять из разных валют");
            }
        }
    }
}
