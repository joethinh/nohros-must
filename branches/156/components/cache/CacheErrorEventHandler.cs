using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Net.Caching
{
    /// <summary>
    /// Defines the signature of the methods that will handle the errors that may occur while
    /// objects are manipulated in the cache.
    /// </summary>
    /// <param name="key">The cache key of the object that cause the error.</param>
    /// <param name="e">The exception that was raised.</param>
    public delegate void CacheErrorEventHandler(ICacheProvider sender, string key, Exception e);
}
