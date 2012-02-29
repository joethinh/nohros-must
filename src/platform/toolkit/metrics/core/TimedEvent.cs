using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// Encapsulates a method whose duration should be timed has no parameters,
  /// returns a result and may throw an exception.
  /// </summary>
  /// <returns>The computed result.</returns>
  /// <exception cref="Exception">If unable to compute a result.</exception>
  /// <remarks></remarks>
  public delegate T TimedEvent<T>();
}
