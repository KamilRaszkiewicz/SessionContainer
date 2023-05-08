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

            builder.Services.AddSessionMappings(Assembly.GetExecutingAssembly());

            var app = builder.Build();
            app.UseSession();

            app.MapGet("/", (NumbersContainer container) =>
            {
                if (container.Numbers == null)
                {
                    container.Numbers = new List<int>() { 1 };
                    container.Save();

                    return container.Numbers;
                }

                container.Numbers.Add(container.Numbers.Last() + 1);
                container.Save();

                return container.Numbers;
            });

            app.Run();
        }
    }
}