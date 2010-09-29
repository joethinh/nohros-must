using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Configuration
{
    public interface IProviderNode
    {
        /// <summary>
        /// Gets the provider name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the assembly-qualified name of the provider type.
        /// </summary>
        /// <seealso cref="AssemblyQualifiedName"/>
        string Type { get; }
    }
}
