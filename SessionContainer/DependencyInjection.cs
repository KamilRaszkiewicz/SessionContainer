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
        public static IServiceCollection AddSessionMappings(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.DefinedTypes
                .Where(t =>
                t.IsAssignableTo(typeof(SessionContainer)) &&
                !t.IsAbstract &&
                !t.IsInterface);

            foreach (var type in types)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }
}
