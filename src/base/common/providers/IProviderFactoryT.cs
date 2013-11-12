using System;
using System.Collections.Generic;

namespace Nohros.Providers
{
  /// <summary>
  /// A generic factory for providers.
  /// </summary>
  public interface IProviderFactory<T>  : IProviderFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="IProviderFactory{T}"/>
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    new T CreateProvider(IDictionary<string, string> options);
  }
}
