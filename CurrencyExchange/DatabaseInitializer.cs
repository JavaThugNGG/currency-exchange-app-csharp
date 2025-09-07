using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace CurrencyExchange
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        private static readonly string CreateCurrenciesTable = @"
            CREATE TABLE currencies (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                code TEXT NOT NULL,
                name TEXT,
                sign TEXT
            );
        ";

        private static readonly string CreateExchangeRatesTable = @"
            CREATE TABLE exchange_rates (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                base_currency_id INTEGER NOT NULL,
                target_currency_id INTEGER NOT NULL,
                rate REAL NOT NULL,
                FOREIGN KEY (base_currency_id) REFERENCES currencies(id),
                FOREIGN KEY (target_currency_id) REFERENCES currencies(id)
            );
        ";

        private static readonly string InsertCurrencies = @"
            INSERT INTO currencies (code, name, sign) VALUES
            ('USD', 'United States Dollar', '$'),
            ('EUR', 'Euro', '€'),
            ('RUB', 'Russian Ruble', '₽');
        ";

        private static readonly string InsertExchangeRates = @"
            INSERT INTO exchange_rates (base_currency_id, target_currency_id, rate) VALUES
            (1, 2, 0.92),   
            (1, 3, 95.50);  
        ";

        private static readonly string SelectCurrencies = @"
            SELECT id, code, name, sign 
            FROM currencies;
        ";

        private static readonly string SelectExchangeRates = @"
            SELECT er.id, c1.code as base, c2.code as target, er.rate
            FROM exchange_rates er
            JOIN currencies c1 ON er.base_currency_id = c1.id
            JOIN currencies c2 ON er.target_currency_id = c2.ID;
        ";

        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Init()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            ExecuteCommand(CreateCurrenciesTable, connection);
            ExecuteCommand(CreateExchangeRatesTable, connection);

            ExecuteCommand(InsertCurrencies, connection);
            ExecuteCommand(InsertExchangeRates, connection);

            CheckInsertedCurrencies(connection);
            CheckInsertedExchangeRates(connection);
        }

        private void ExecuteCommand(string sql, SqliteConnection connection)
        {
            try
            {
                Debug.WriteLine("[SQL EXEC] " + sql);
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Debug.WriteLine("[SQLite ERROR] " + ex.Message);
                throw;
            }
        }

        private void CheckInsertedCurrencies(SqliteConnection connection)
        {
            Debug.WriteLine($"[SQL EXEC] {SelectCurrencies}");
            using var selectCurrenciesCommand = connection.CreateCommand();
            selectCurrenciesCommand.CommandText = SelectCurrencies;
            using var currenciesReader = selectCurrenciesCommand.ExecuteReader();
            while (currenciesReader.Read())
            {
                var id = currenciesReader.GetInt64(currenciesReader.GetOrdinal("id"));
                var code = currenciesReader.GetString(currenciesReader.GetOrdinal("code"));
                var name = currenciesReader.GetString(currenciesReader.GetOrdinal("name"));
                var sign = currenciesReader.GetString(currenciesReader.GetOrdinal("sign"));
                Debug.WriteLine($"ID: {id}, Code: {code}, Name: {name}, Sign: {sign}");
            }
        }

        private void CheckInsertedExchangeRates(SqliteConnection connection)
        {
            Debug.WriteLine("[CHECK] exchange_rates:");
            using var selectRatesCommand = connection.CreateCommand();
            selectRatesCommand.CommandText = SelectExchangeRates;
            using var ratesReader = selectRatesCommand.ExecuteReader();
            while (ratesReader.Read())
            {
                Debug.WriteLine($"ID: {ratesReader.GetInt32(0)}, {ratesReader.GetString(1)} -> {ratesReader.GetString(2)}, Rate: {ratesReader.GetDouble(3)}");
            }
        }
    }
}
