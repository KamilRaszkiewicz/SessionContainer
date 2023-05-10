using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SessionContainer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSessionMappings(this IServiceCollection services)
        {
            var types = typeof(SessionContainer).Assembly
                .GetReferencingAssemblies()
                .SelectMany(a => a.DefinedTypes)
                .Where(t =>
                    t.IsAssignableTo(typeof(SessionContainer)) &&
                    !t.IsAbstract &&
                    !t.IsInterface);

            foreach (var type in types)
            {
                services.AddScoped(type);
            }

            return services;
        }
    }
}
