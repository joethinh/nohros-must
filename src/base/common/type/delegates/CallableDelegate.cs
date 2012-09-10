using System;

namespace Nohros
{
  /// <summary>
  /// A task that returns a result and may throw an exception.
  /// </summary>
  /// <remarks>
  /// A <see cref="CallableDelegate{T}"/> computes a result, or throws an
  /// exception if unable to do so.
  /// </remarks>
  /// <typeparam name="T">
  /// The type of the returning object.
  /// </typeparam>
  /// <returns>
  /// The computed result.
  /// </returns>
  /// <exception cref="Exception">
  /// If unable to compute a result.
  /// </exception>
  public delegate T CallableDelegate<out T>();
}
