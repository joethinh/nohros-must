using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// Contains the configuration data related with query definition.
  /// </summary>
  public interface IQuerySettings : ISettings
  {
    long QueryCacheDuration { get; }
  }
}
