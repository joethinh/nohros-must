using System;

namespace Nohros
{
  /// <summary>
  /// A class that can supply objects of a single type. Semantically, this
  /// could be a factory, generator, builder, or something else entirely. No
  /// guarantess are implied by this interface.
  /// </summary>
  public interface ISupplier<T>
  {
    /// <summary>
    /// Gets an instance of the appropriate type. The returned object may or
    /// not be a new instance, depending on the implementation.
    /// </summary>
    /// <remarks>
    /// There is no guaranteee that the supplier will always supplies a valid
    /// object. This is implementation specific.
    /// </remarks>
    T Supply();
  }
}