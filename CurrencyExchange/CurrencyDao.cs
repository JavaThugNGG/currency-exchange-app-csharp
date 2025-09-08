using Microsoft.Data.Sqlite;

namespace CurrencyExchange
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
                WHERE code = @code
            ;";

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
    }
}