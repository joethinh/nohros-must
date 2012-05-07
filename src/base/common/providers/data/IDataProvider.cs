using System;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Allows an object to implements a SQLDataProvider, and represents a set of
  /// methods and properties used to query a data store.
  /// </summary>
  [Obsolete("This interface is deprecated.")]
  public interface IDataProvider
  {
    /// <summary>
    /// Gets the name of the data provider.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the string tha can be used to make a connection to the data
    /// provider.
    /// </summary>
    string ConnectionString { get; }
  }
}