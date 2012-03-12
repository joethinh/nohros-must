using System;

namespace Nohros.Caching
{
  /// <summary>
  /// Defines the signature of the method that are used to populate a
  /// <see cref="Cache&lt;V&gt;"/>.
  /// </summary>
  public delegate T CacheLoaderDelegate<T>(string key);
}