using CurrencyExchange.Dto;
using Microsoft.Data.Sqlite;
using CurrencyExchange.Exceptions;

namespace CurrencyExchange.Dao
{
    public class CurrencyDao
    {
        private readonly string _connectionString;

        public CurrencyDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public CurrencyDto GetCurrency(string code)
        {
            var query = @"
                SELECT *
                FROM currencies 
                WHERE code = @code;
            ";

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var command = new SqliteCommand(query, connection);
                command.Parameters.Add("@code", SqliteType.Text).Value = code;

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new CurrencyDto(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetString(reader.GetOrdinal("name")),
                        reader.GetString(reader.GetOrdinal("code")),
                        reader.GetString(reader.GetOrdinal("sign"))
                    );
                }

                throw new ElementNotFoundException("Запрашиваемая валюта не найдена!");
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException("Ошибка при работе с БД!", ex);
            }
        }

        public IList<CurrencyDto> GetAllCurrencies()
        {
            IList<CurrencyDto> currencies = new List<CurrencyDto>();
            var query = @"
                SELECT *
                FROM currencies;
            ";

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var command = new SqliteCommand(query, connection);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var currency = new CurrencyDto
                    (
                        reader.GetInt32(reader.GetOrdinal("id")),//Маппер потом сделаешь
                        reader.GetString(reader.GetOrdinal("name")),
                        reader.GetString(reader.GetOrdinal("code")),
                        reader.GetString(reader.GetOrdinal("sign"))
                    );
                    currencies.Add(currency);
                }
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException("Ошибка при работе с БД!", ex);
            }

            return currencies;
        }

        public long InsertCurrency(string name, string code, string sign)
        {
            string query = @"
                INSERT INTO currencies (name, code, sign)
                VALUES (@name, @code, @sign)
                RETURNING id;
            ";

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var command = new SqliteCommand(query, connection);
                command.Parameters.Add("@name", SqliteType.Text).Value = name;
                command.Parameters.Add("@code", SqliteType.Text).Value = code;
                command.Parameters.Add("@sign", SqliteType.Text).Value = sign;

                return (long)command.ExecuteScalar()!;
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException("Ошибка при работе с БД!", ex);
            }
        }

        public bool IsCurrencyExists(string code)
        {
            string query = @"
                SELECT COUNT(*)
                FROM currencies
                WHERE code = @code;
            ";

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                using var command = new SqliteCommand(query, connection);
                command.Parameters.Add("@code", SqliteType.Text).Value = code;

                var count = (long)command.ExecuteScalar()!;

                return count > 0;
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException("Ошибка при работе с базой данных!", ex);
            }
        }
    }
}