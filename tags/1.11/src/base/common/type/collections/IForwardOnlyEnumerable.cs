using System;
using System.Collections.Generic;

namespace Nohros.Collections
{
  /// <summary>
  /// A <see cref="IEnumerable{T}"/> that supports only fetching the elements
  /// serially from the start to the end of the collection.
  /// </summary>
  /// <remarks>
  /// A <see cref="IForwardOnlyEnumerable{T}"/> is tipically used for
  /// collections that performs a lazy-load of its elements. The elements are
  /// not loaded until they are fetched.
  /// </remarks>
  public interface IForwardOnlyEnumerable<T> : IEnumerable<T>
  {
  }
}
