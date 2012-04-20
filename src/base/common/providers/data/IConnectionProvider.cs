using System;
using System.Collections.Generic;
using System.Data;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Provides an interface for getting connection to a data store.
  /// </summary>
  /// <typeparam name="T">
  /// The type of connection that is returned by the
  /// <see cref="CreateConnection"/> method.
  /// </typeparam>
  public interface IConnectionProvider<out T> where T: IDbConnection
  {
    /// <summary>
    /// Creates an instance of the <see cref="IDbConnection"/> class.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="IDbConnection"/> object.
    /// </returns>
    T CreateConnection();
  }
}
