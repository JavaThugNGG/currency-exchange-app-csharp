namespace CurrencyExchange
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                var startup = new Startup();

                startup.ConfigureServices(builder);

                var app = builder.Build();

                startup.ConfigureMiddleware(app);
                startup.ConfigureRouting(app);
                startup.ConfigureApplicationLifetime(app);

                var dbInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
                dbInitializer.Init();

                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запуске приложения: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}