using System.Reflection;

namespace SessionContainer.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSessionMappings();

            var app = builder.Build();
            app.UseSession();

            app.MapGet("/", (NumbersContainer container) =>
            {
                container.Numbers.Add(container.Numbers.LastOrDefault() + 1);

                container.Save();

                return container.Numbers;
            });

            app.Run();
        }
    }
}