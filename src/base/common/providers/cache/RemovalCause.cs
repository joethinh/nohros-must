using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching
{
  /// <summary>
  /// The reason why a cached entry was removed.
  /// </summary>
  public enum RemovalCause
  {
    /// <summary>
    /// The entry was manually removed by the user. This can result from the
    /// user invoking <see cref="Cache.Remove"/>
    /// </summary>
    Explicit = 0,

    /// <summary>
    /// The entry was removed automatically because its key or value was
    /// garbage-collected. This can occur when the cache is full.
    /// </summary>
    Collected = 1,

    /// <summary>
    /// The entry's expiration timestamp has passed.
    /// </summary>
    Expired = 2
  }
}
