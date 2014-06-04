using System;

namespace Nohros.Data
{
  /// <summary>
  /// Defines a range of identities values using the hilo algorithm.
  /// </summary>
  public interface IHiLoRange
  {
    /// <summary>
    /// Get the high value of the <see cref="HiLoRange"/> object.
    /// </summary>
    long High { get; }

    /// <summary>
    /// Get the maximum number of low values that could be generated using the
    /// <see cref="High"/> value.
    /// </summary>
    int MaxLow { get; }
  }
}
