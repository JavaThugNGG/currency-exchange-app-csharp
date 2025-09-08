using Microsoft.Data.Sqlite;

namespace CurrencyExchange
{
    public class ExchangeRateMapper
    {
        public ExchangeRateDto ToDto(SqliteDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("rateId"));

            var baseCurrency = new CurrencyDto(
                reader.GetInt64(reader.GetOrdinal("baseId")),
                reader.GetString(reader.GetOrdinal("baseName")),
                reader.GetString(reader.GetOrdinal("baseCode")),
                reader.GetString(reader.GetOrdinal("baseSign"))
            );

            var targetCurrency = new CurrencyDto(
                reader.GetInt64(reader.GetOrdinal("targetId")),
                reader.GetString(reader.GetOrdinal("targetName")),
                reader.GetString(reader.GetOrdinal("targetCode")),
                reader.GetString(reader.GetOrdinal("targetSign"))
            );

            decimal rate = Convert.ToDecimal(reader.GetDouble(reader.GetOrdinal("rate")));

            return new ExchangeRateDto(id, baseCurrency, targetCurrency, rate);
        }
    }
}
