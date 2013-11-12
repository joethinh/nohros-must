using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Providers
{
  /// <summary>
  /// A factory for providers.
  /// </summary>
  public interface IProviderFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="IProviderFactory{T}"/>
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    object CreateProvider(IDictionary<string, string> options);
  }
}
