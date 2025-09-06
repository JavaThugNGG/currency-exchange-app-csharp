using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Diagnostics;

namespace CurrencyExchange
{
    class DatabaseInitializer
    {
        public static void Init()
        {
            Batteries.Init(); // важно для SQLitePCLRaw

            var connectionString = "Data Source=:memory:"; // база в памяти
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            void ExecuteCommand(string sql)
            {
                try
                {
                    Debug.WriteLine("[SQL EXEC] " + sql);
                    using var cmd = connection.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    Debug.WriteLine("[SQLite ERROR] " + ex.Message);
                }
            }

            // Создание таблицы currencies
            ExecuteCommand(@"
                CREATE TABLE IF NOT EXISTS currencies (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Code TEXT NOT NULL,
                    FullName TEXT,
                    Sign TEXT
                );
            ");

            // Создание таблицы exchange_rates
            ExecuteCommand(@"
                CREATE TABLE IF NOT EXISTS exchange_rates (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    BaseCurrencyId INTEGER NOT NULL,
                    TargetCurrencyId INTEGER NOT NULL,
                    Rate REAL NOT NULL,
                    FOREIGN KEY (BaseCurrencyId) REFERENCES currencies(ID),
                    FOREIGN KEY (TargetCurrencyId) REFERENCES currencies(ID)
                );
            ");

            // Вставка базовых валют
            ExecuteCommand(@"
                INSERT INTO currencies (Code, FullName, Sign) VALUES
                ('USD', 'United States Dollar', '$'),
                ('EUR', 'Euro', '€'),
                ('RUB', 'Russian Ruble', '₽');
            ");

            // Вставка базовых курсов обмена
            ExecuteCommand(@"
                INSERT INTO exchange_rates (BaseCurrencyId, TargetCurrencyId, Rate) VALUES
                (1, 2, 0.92),   -- USD -> EUR
                (1, 3, 95.50),  -- USD -> RUB
            ");

            // Проверка вставки валют
            Debug.WriteLine("[SQL EXEC] SELECT ID, Code, FullName, Sign FROM currencies;");
            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT ID, Code, FullName, Sign FROM currencies;";
            using var currenciesReader = selectCmd.ExecuteReader();
            while (currenciesReader.Read())
            {
                var id = currenciesReader.GetInt32(0);
                var code = currenciesReader.GetString(1);
                var fullName = currenciesReader.GetString(2);
                var sign = currenciesReader.GetString(3);
                Debug.WriteLine($"ID: {id}, Code: {code}, FullName: {fullName}, Sign: {sign}");
            }

            // Проверка вставки курсов
            Debug.WriteLine("[CHECK] exchange_rates:");
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT er.ID, c1.Code as Base, c2.Code as Target, er.Rate
                    FROM exchange_rates er
                    JOIN currencies c1 ON er.BaseCurrencyId = c1.ID
                    JOIN currencies c2 ON er.TargetCurrencyId = c2.ID;
                ";
                using var ratesReader = cmd.ExecuteReader();
                while (ratesReader.Read())
                {
                    Debug.WriteLine($"ID: {ratesReader.GetInt32(0)}, {ratesReader.GetString(1)} -> {ratesReader.GetString(2)}, Rate: {ratesReader.GetDouble(3)}");
                }
            }
        }
    }
}
