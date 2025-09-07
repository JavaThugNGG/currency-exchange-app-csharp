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
            string query = "SELECT * FROM currencies WHERE code = @code;";

            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@code", code);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CurrencyDto(
                                Id: reader.GetInt32(reader.GetOrdinal("id")),
                                Name: reader.GetString(reader.GetOrdinal("name")),
                                Code: reader.GetString(reader.GetOrdinal("code")),
                                Sign: reader.GetString(reader.GetOrdinal("sign"))
                            );
                        }
                        else
                        {
                            throw new ElementNotFoundException("Запрашиваемая валюта не найдена");
                        }
                    }
                }
            }
        }
    }
}
