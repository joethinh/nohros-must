using System;
using System.Collections.Generic;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// A class that maps the fields of a <see cref="IDataReader"/> to the
  /// properties of an instance of the type <typeparamref name="T"/>.
  /// </summary>
  /// <typeparam name="T">
  /// The type that should be mapped.
  /// </typeparam>
  public interface IDataReaderMapper<T>
  {
    /// <summary>
    /// Maps a element of a query result to a object of type
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <returns>
    /// A object of type <typeparamref name="T"/> containing the data readed
    /// from the current row of the given <paramref name="reader"/>.
    /// </returns>
    T Map(IDataReader reader);

    /// <summary>
    /// Maps a element of a query result to a collection of object of type
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <returns>
    /// A collection of objects of type <typeparamref name="T"/> containing
    /// the data readed from the given <paramref name="reader"/>.
    /// </returns>
    IEnumerable<T> Map(IDataReader reader, bool defer);
  }
}
