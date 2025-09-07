namespace CurrencyExchange
{
    public class Program
    {
        internal static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found!");

            builder.Services.AddControllers();
            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.MapControllers();

            var dbInitializer = new DatabaseInitializer(connectionString);
            dbInitializer.Init();
            
            app.Run();
        }
    }
}