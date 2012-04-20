using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Nohros.Configuration;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// A factory used to create instances of the
  /// <see cref="IConnectionProvider{T}"/> class.
  /// </summary>
  public interface IConnectionProviderFactory<T> where T: IDbConnection
  {
    /// <summary>
    /// Creates an instance of the <see cref="IConnectionProvider{T}"/> class
    /// by using the configured provider options.
    /// </summary>
    /// <param name="options">
    /// A collection of name value pairs representing the options configured
    /// for a specific provider.
    /// </param>
    /// <returns>
    /// A <see cref="IConnectionProvider{T}"/> object.
    /// </returns>
    /// <exception cref="ProviderException">
    /// An instance of the required provider could not be created. If a
    /// exception is raised dureing the provider instantiation, it should be
    /// wrapped in a <see cref="ProviderException"/>.
    /// </exception>
    IConnectionProvider<T> CreateProvider(IDictionary<string, string> options);
  }
}
