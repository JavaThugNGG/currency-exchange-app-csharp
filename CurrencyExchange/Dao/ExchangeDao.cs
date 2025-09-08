using CurrencyExchange.Dto;
using CurrencyExchange.Mappers;
using Microsoft.Data.Sqlite;
using CurrencyExchange.Exceptions;

namespace CurrencyExchange.Dao
{
    public class ExchangeDao
    {
        private readonly string _connectionString;
        private readonly RawExchangeMapper _rawExchangeMapper;

        public ExchangeDao(string connectionString, RawExchangeMapper rawExchangeMapper)
        {
            _connectionString = connectionString;
            _rawExchangeMapper = rawExchangeMapper;
        }

        public RawExchangeDto GetRate(string baseCurrencyCode, string targetCurrencyCode, decimal amount)
        {
            string query = @"
                SELECT c1.id AS baseId,
                    c1.name AS baseName,
                    c1.code AS baseCode,
                    c1.sign AS baseSign,
                    c2.id AS targetId,
                    c2.name AS targetName,
                    c2.code AS targetCode,
                    c2.sign AS targetSign,
                    rate
                FROM exchange_rates er
                JOIN currencies c1 ON er.base_currency_id = c1.id
                JOIN currencies c2 ON er.target_currency_id = c2.id
                WHERE c1.code = @baseCurrencyCode AND c2.code = @targetCurrencyCode;
            ";

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var command = new SqliteCommand(query, connection);
                command.Parameters.Add("@baseCurrencyCode", SqliteType.Text).Value = baseCurrencyCode;
                command.Parameters.Add("@targetCurrencyCode", SqliteType.Text).Value = targetCurrencyCode;

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return _rawExchangeMapper.ToDto(reader, amount);
                }
                throw new ElementNotFoundException("Запрашиваемый элемент не найден");
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException("Ошибка при работе с базой данных", ex);
            }
        }

    }
}
