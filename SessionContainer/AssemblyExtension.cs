using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SessionContainer
{
    public static class AssemblyExtension
    {
        /// <summary>
        /// Get array of assemblies that reference the assembly, including the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetReferencingAssemblies(this Assembly assembly)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            yield return assembly;

            foreach(var loadedAssembly in loadedAssemblies)
            {
                var referencedAssemblies = loadedAssembly.GetReferencedAssemblies();

                foreach(var referencedAssembly in referencedAssemblies)
                {
                    if(referencedAssembly.FullName ==  assembly.FullName)
                    {
                        yield return loadedAssembly;
                        break;
                    }
                }
            }
        }
    }
}
