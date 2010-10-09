using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Net.Caching
{
    /// <summary>
    /// Defines methods and properties of the temporary objects. Temporary objects are objects
    /// that exists for a limited amount of time.
    /// </summary>
    public interface ITemporaryObject
    {
        /// <summary>
        /// The name of the temporary object.
        /// </summary>
        string Name { get; }
    }
}