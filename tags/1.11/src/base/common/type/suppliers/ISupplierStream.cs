using System;

namespace Nohros
{
  /// <summary>
  /// A implementation of the <see cref="ISupplier{T}"/> class that supply
  /// elements of type <typeparamref name="T"/> in a sequence.
  /// </summary>
  /// <typeparam name="T">
  /// The type of objects supplied.
  /// </typeparam>
  public interface ISupplierStream<T> : ISupplier<T>
  {
    /// <summary>
    /// Gets a value indicating if an item available is available from the
    /// supplier.
    /// </summary>
    /// <value>
    /// <c>true</c> if an item is available from the supplier(that is, if
    /// <see cref="ISupplier{T}.Supply"/> method wolud return a value);
    /// otherwise <c>false</c>.
    /// </value>
    bool Available { get; }
  }
}
