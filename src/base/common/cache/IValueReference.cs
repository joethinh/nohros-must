using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching
{
  /// <summary>
  /// A reference to a value.
  /// </summary>
  /// <typeparam name="T">The type of the object that this instance reference.
  /// </typeparam>
  internal interface IValueReference<out T>
  {
    /// <summary>
    /// Gets the value
    /// </summary>
    /// <remarks>This property should not block ot throw exceptions.</remarks>
    T Value { get; }

    /// <summary>
    /// Waits for a value that still be loading. Unlike <see cref="Value"/>,
    /// this method can block(in the case of FutureValueReference).
    /// </summary>
    /// <returns></returns>
    /// <exception cref="TypeLoadException"> if the loading thread throws an
    /// exception.</exception>
    T WaitForValue();

    /// <summary>
    /// Gets a value indicating if a new value is currently loading,
    /// regardless or not there is an existing value. It is assumed that the
    /// return value of this method is constant for any given instance
    /// <see cref="IValueReference{T}"/> instance.
    /// </summary>
    bool IsLoading { get; }
  }
}
