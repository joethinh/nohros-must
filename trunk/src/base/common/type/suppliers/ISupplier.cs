using System;
using System.Collections.Generic;
using System.Text;

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
    T Supply();
  }
}