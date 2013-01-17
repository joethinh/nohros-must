using System;
using System.Collections.Generic;
using System.Data;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Provides an interface for getting connection to a data store.
  /// </summary>
  public interface IConnectionProvider
  {
    /// <summary>
    /// Creates an instance of the <see cref="IDbConnection"/> class.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="IDbConnection"/> object.
    /// </returns>
    IDbConnection CreateConnection();

    /// <summary>
    /// Creates an instance of the <see cref="ITransactionContext"/> class for
    /// the current <see cref="IConnectionProvider"/>.
    /// </summary>
    /// <returns>
    /// The newly created <see cref="ITransactionContext"/>.
    /// </returns>
    ITransactionContext CreateTransactionContext();

    /// <summary>
    /// Gets an string that represents the database schema related with the
    /// provider.
    /// </summary>
    /// <remarks>
    /// A database schema is a logical data structure used to organize the
    /// objects from a database.
    /// <para>
    /// If the database schema is not specified this method should returns the
    /// default database schema.
    /// </para>
    /// <para>
    /// If the provider does not have the concept of schemas this method should
    /// returns a empty string.
    /// </para>
    /// </remarks>
    string Schema { get; }
  }
}
