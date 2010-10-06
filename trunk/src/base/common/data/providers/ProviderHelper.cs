using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Nohros.Resources;
using Nohros.Configuration;

namespace Nohros.Data
{
    /// <summary>
    /// Defines helper methods used by providers.
    /// </summary>
    internal sealed class ProviderHelper
    {
        /// <summary>
        /// Gets the <see cref="Type"/> of a provider, using the specified provider node.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns>The type instance that represents the exact runtime type of the specified provider or null
        /// if the type could not be loaded.</returns>
        public static Type GetTypeFromProviderNode(ProviderNode provider) {
            if (provider == null)
                throw new ArgumentNullException("provider");

            Type type = null;
            // attempt to load .NET type of the provider. If the location of the assemlby is specified we
            // need to load the assemlby and try to get the type from the loaded assembly. The name of the
            // assembly will be extracted from the provider type.
            if (provider.AssemblyLocation != null) {
                string assembly_name = provider.Type;
                int num = assembly_name.IndexOf(',');
                if (num == -1)
                    throw new ProviderException(string.Format(StringResources.Provider_LoadAssembly, provider.AssemblyLocation));

                assembly_name = assembly_name.Substring(num + 1).Trim();
                int num2 = assembly_name.IndexOfAny(new char[] { ' ', ',' });
                if (num2 != -1)
                    assembly_name = assembly_name.Substring(0, num2);

                if (!assembly_name.EndsWith(".dll"))
                    assembly_name = assembly_name + ".dll";

                string assembly_path = Path.Combine(provider.AssemblyLocation, assembly_name);
                if (!File.Exists(assembly_path))
                    throw new ProviderException(string.Format(StringResources.Provider_LoadAssembly, assembly_path));

                try {
                    Assembly assembly = Assembly.LoadFrom(assembly_path);
                    type = assembly.GetType(provider.Type.Substring(0, num));
                } catch (Exception ex) {
                    throw new ProviderException(string.Format(StringResources.Provider_LoadAssembly, assembly_path), ex);
                }
            }
            else
                type = Type.GetType(provider.Type);

            return type;
        }
    }
}