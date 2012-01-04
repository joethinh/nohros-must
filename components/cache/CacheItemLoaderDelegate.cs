using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Net.Caching
{
    /// <summary>
    /// Defines the signature of the methods that are used to automatically instantiate objects
    /// while a cache update is required.
    /// </summary>
    /// <returns></returns>
    public delegate object CacheItemLoaderDelegate();
}
