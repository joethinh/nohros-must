using System;
using System.Collections.Generic;

namespace Nohros.Data
{
  /// <summary>
  /// Provides a means of mapping a result to a object.
  /// </summary>
  /// <typeparam name="T">
  /// The type of the object to map.
  /// </typeparam>
  public interface IMapper<T> : IEnumerable<T>
  {
    /// <summary>
    /// Maps a element of a query result to a object of type
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <returns>
    /// A object of type <typeparamref name="T"/> containing the data of the
    /// current element of a record set.
    /// </returns>
    T Map();
  }
}
