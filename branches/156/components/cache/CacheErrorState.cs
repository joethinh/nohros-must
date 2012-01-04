using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Net.Caching
{
    /// <summary>
    /// Specifies the state of the cache when an error was occured.
    /// </summary>
    public enum CacheErrorState
    {
        /// <summary>
        /// The error has occurred during the execution of a cache item removed callback method.
        /// </summary>
        CacheItemRemoved = 0,

        /// <summary>
        /// The error has occurred during the excution of a cache item load method.
        /// </summary>
        CacheItemLoad = 1
    }
}