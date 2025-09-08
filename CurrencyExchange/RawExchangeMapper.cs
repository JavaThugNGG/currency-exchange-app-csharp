using Microsoft.Data.Sqlite;

namespace CurrencyExchange
{
    public class RawExchangeMapper
    {
        public RawExchangeDto ToDto(SqliteDataReader reader, decimal amount)
        {
            CurrencyDto baseCurrency = new CurrencyDto(
                reader.GetInt32(reader.GetOrdinal("baseId")),
                reader.GetString(reader.GetOrdinal("baseName")),
                reader.GetString(reader.GetOrdinal("baseCode")),
                reader.GetString(reader.GetOrdinal("baseSign")));

            CurrencyDto targetCurrency = new CurrencyDto(
                reader.GetInt32(reader.GetOrdinal("targetId")),
                reader.GetString(reader.GetOrdinal("targetName")),
                reader.GetString(reader.GetOrdinal("targetCode")),
                reader.GetString(reader.GetOrdinal("targetSign")));

            decimal rate = reader.GetDecimal(reader.GetOrdinal("rate"));

            return new RawExchangeDto(baseCurrency, targetCurrency, rate, amount);
        }
    }
}
