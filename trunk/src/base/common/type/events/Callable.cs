using System;

namespace Nohros
{
  /// <summary>
  /// A task that returns a result and may throw an exception.
  /// </summary>
  /// <typeparam name="T">
  /// The result type of delegate call.
  /// </typeparam>
  public delegate T Callable<out T>();
}
