using CurrencyExchange.Dto;
using CurrencyExchange.Mappers;
using Microsoft.Data.Sqlite;
using CurrencyExchange.Exceptions;

namespace CurrencyExchange.Dao
{
    public class ExchangeRateDao
    {
        private readonly string _connectionString;
        private readonly ExchangeRateMapper _exchangeRateMapper;

        public ExchangeRateDao(string connectionString, ExchangeRateMapper exchangeRateMapper)
        {
            _connectionString = connectionString;
            _exchangeRateMapper = exchangeRateMapper;
        }

        public IList<ExchangeRateDto> GetAllRates()
        {
            IList<ExchangeRateDto> exchangeRates = new List<ExchangeRateDto>();
            string query = @"
                SELECT er.id AS rateId,
                    er.rate AS rate,
                    c1.id AS baseId,
                    c1.code AS baseCode,
                    c1.name AS baseName,
                    c1.sign AS baseSign,
                    c2.id AS targetId,
                    c2.code AS targetCode,
                    c2.name AS targetName,
                    c2.sign AS targetSign
                FROM exchange_rates er
                JOIN currencies c1 ON er.base_currency_id = c1.id
                JOIN currencies c2 ON er.target_currency_id = c2.id;
            ";

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var command = new SqliteCommand(query, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    exchangeRates.Add(_exchangeRateMapper.ToDto(reader));
                }
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException("Ошибка при доступе к БД!", ex);
            }
            return exchangeRates;
        }

        public ExchangeRateDto GetRate(string baseCurrencyCode, string targetCurrencyCode)
        {
            string query = @"
                SELECT er.id AS rateId,
                       er.rate AS rate,
                       c1.id AS baseId,
                       c1.name AS baseName,
                       c1.code AS baseCode,
                       c1.sign AS baseSign,
                       c2.id AS targetId,
                       c2.name AS targetName,
                       c2.code AS targetCode,
                       c2.sign AS targetSign
                FROM exchange_rates er
                JOIN currencies c1 ON er.base_currency_id = c1.id
                JOIN currencies c2 ON er.target_currency_id = c2.id
                WHERE c1.code = @baseCode AND c2.code = @targetCode;
            ";

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = new SqliteCommand(query, connection);

            command.Parameters.Add("@baseCode", SqliteType.Text).Value = baseCurrencyCode;
            command.Parameters.Add("@targetCode", SqliteType.Text).Value = targetCurrencyCode;

            using var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                return _exchangeRateMapper.ToDto(reader);
            }
            throw new ElementNotFoundException("Запрашиваемый элемент не найден");
        }

        public void UpdateRate(string baseCurrencyCode, string targetCurrencyCode, decimal rate)
        {
            string query = @"
                UPDATE exchange_rates
                SET rate = @rate
                WHERE base_currency_id = (SELECT id FROM currencies WHERE code = @baseCode)
                AND target_currency_id = (SELECT id FROM currencies WHERE code = @targetCode);
            ";

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = new SqliteCommand(query, connection);

            command.Parameters.Add("@rate", SqliteType.Real).Value = rate;
            command.Parameters.Add("@baseCode", SqliteType.Text).Value = baseCurrencyCode;
            command.Parameters.Add("@targetCode", SqliteType.Text).Value = targetCurrencyCode;

            command.ExecuteNonQuery();
        }

        public void InsertRate(string baseCurrencyCode, string targetCurrencyCode, decimal rate)
        {
            string query = @"
                INSERT INTO exchange_rates (base_currency_id, target_currency_id, rate)
                VALUES (
                    (SELECT id FROM currencies WHERE code = @baseCode),
                    (SELECT id FROM currencies WHERE code = @targetCode),
                    @rate
                );
            ";

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = new SqliteCommand(query, connection);

            command.Parameters.Add("@baseCode", SqliteType.Text).Value = baseCurrencyCode;
            command.Parameters.Add("@targetCode", SqliteType.Text).Value = targetCurrencyCode;
            command.Parameters.Add("@rate", SqliteType.Real).Value = rate;

            command.ExecuteNonQuery();
        }

        public bool IsRateExists(string baseCurrencyCode, string targetCurrencyCode)
        {
            string query = @"
                SELECT id
                FROM exchange_rates
                WHERE base_currency_id = (SELECT id FROM currencies WHERE code = @baseCode)
                  AND target_currency_id = (SELECT id FROM currencies WHERE code = @targetCode);
            ";

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = new SqliteCommand(query, connection);

            command.Parameters.Add("@baseCode", SqliteType.Text).Value = baseCurrencyCode;
            command.Parameters.Add("@targetCode", SqliteType.Text).Value = targetCurrencyCode;

            var result = command.ExecuteScalar();
            return result != null && Convert.ToInt64(result) > 0;
        }
    }
}
