namespace CurrencyExchange
{
    public class DatabaseConnectionManager : IDisposable
    {
        private readonly string _connectionString;
        public Microsoft.Data.Sqlite.SqliteConnection Connection { get; private set; }

        public DatabaseConnectionManager(string connectionString)
        {
            _connectionString = connectionString;
            Connection = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString);
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
        }
    }

}
