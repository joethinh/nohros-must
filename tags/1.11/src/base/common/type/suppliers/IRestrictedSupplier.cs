using System;

namespace Nohros
{
  /// <summary>
  /// A <see cref="ISupplier{T}"/> that restrict the number of elements that
  /// can be supplied.
  /// </summary>
  public interface IRestrictedSupplier<T> : ISupplier<T>
  {
    /// <summary>
    /// Gets an instance of the appropriate type. The returned object may or
    /// not be a new instance, depending on the implementation.
    /// </summary>
    /// <remarks>
    /// There is no guaranteee that the supplier will always supplies a valid
    /// object. This is implementation specific.
    /// </remarks>
    T Supply(int limit);
  }
}
