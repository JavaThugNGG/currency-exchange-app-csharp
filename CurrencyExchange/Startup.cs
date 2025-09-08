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

            builder.Services.AddSingleton(connectionString);
            builder.Services.AddSingleton(dbConnectionManager);
            builder.Services.AddSingleton<DatabaseInitializer>();
            builder.Services.AddSingleton<CurrencyDao>();
            builder.Services.AddSingleton<RawExchangeMapper>();
            builder.Services.AddSingleton<ExchangeMapper>();
            builder.Services.AddSingleton<ExchangeDao>();
            builder.Services.AddSingleton<ExchangeRateDao>();
            builder.Services.AddSingleton<CurrencyService>();
            builder.Services.AddSingleton<ExchangeService>();
            builder.Services.AddSingleton<ExchangeRateService>();
            builder.Services.AddSingleton<CurrencyValidator>();
            builder.Services.AddSingleton<ExchangeValidator>();
            builder.Services.AddSingleton<ExchangeRateValidator>();
            builder.Services.AddSingleton<ExchangeRateProcessor>();
            builder.Services.AddSingleton<ExchangeProcessor>();
            builder.Services.AddSingleton<ExchangeRateMapper>();
        }

        public void ConfigureMiddleware(WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
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