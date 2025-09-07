namespace CurrencyExchange
{
    public class Startup
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("Default")
                                   ?? throw new InvalidOperationException("Connection string 'Default' not found!");

            var dbConnectionManager = new DatabaseConnectionManager(connectionString);
            dbConnectionManager.OpenPersistent();

            var dbInitializer = new DatabaseInitializer(connectionString);
            var currencyDao = new CurrencyDao(connectionString);

            builder.Services.AddSingleton(dbConnectionManager);
            builder.Services.AddSingleton(dbInitializer);
            builder.Services.AddSingleton(currencyDao);
            builder.Services.AddSingleton<CurrencyService>();
            builder.Services.AddSingleton<CurrencyValidator>();
            builder.Services.AddSingleton<CurrencyProcessor>();
        }

        public void ConfigureMiddleware(WebApplication app)
        {
            ConfigureStaticFiles(app);
        }

        public void ConfigureRouting(WebApplication app)
        {
            app.MapControllers();
        }

        public void ConfigureApplicationLifetime(WebApplication app)
        {
            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            var dbManager = app.Services.GetRequiredService<DatabaseConnectionManager>();

            lifetime.ApplicationStopping.Register(() =>
            {
                dbManager.Dispose();
            });
        }

        private void ConfigureStaticFiles(WebApplication app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}