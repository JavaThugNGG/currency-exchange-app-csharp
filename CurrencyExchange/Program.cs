namespace CurrencyExchange
{
    public class Program
    {
        internal static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.MapControllers();

            DatabaseTest.Test();

            app.Run();
        }
    }
}