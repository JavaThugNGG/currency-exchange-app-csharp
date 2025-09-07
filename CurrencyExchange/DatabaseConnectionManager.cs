namespace CurrencyExchange
{
    public class DatabaseConnectionManager : IDisposable
    {
        private readonly string _connectionString;
        private readonly Microsoft.Data.Sqlite.SqliteConnection _connection;

        public DatabaseConnectionManager(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString);
        }

        public void OpenPersistent()
        {
            _connection.Open();
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }

}
