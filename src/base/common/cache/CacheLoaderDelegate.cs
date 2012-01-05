using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching
{
  /// <summary>
  /// Defines the signature of the method that are used to populate a
  /// <see cref="Cache&lt;V&gt;"/>.
  /// </summary>
  public delegate V CacheLoaderDelegate<V>(string key);
}
