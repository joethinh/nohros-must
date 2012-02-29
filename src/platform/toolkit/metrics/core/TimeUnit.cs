using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros
{
  /// <summary>
  /// A <see cref="TimeUnit"/> represents the duration at a given unit of
  /// granularity. <see cref="TimeUnit"/> helps organize and use time
  /// representations that may be maintened separately across various context.
  /// </summary>
  public enum TimeUnit
  {
    /// <summary>
    /// A nanosecond is defined as one thousandth of a microsecond.
    /// </summary>
    Nanoseconds = 0,

    /// <summary>
    /// A microsecond is defined as thousandth of a millisecond.
    /// </summary>
    Microseconds = 1,

    /// <summary>
    /// A millisecond is defined as thousandth of a second.
    /// </summary>
    Miliseconds = 2,

    /// <summary>
    /// The base unit of mensure.
    /// </summary>
    Seconds = 3,

    /// <summary>
    /// A minute is defined as sixty seconds.
    /// </summary>
    Minutes = 4,

    /// <summary>
    /// A hour is defined as sixty minutes.
    /// </summary>
    Hours = 5,

    /// <summary>
    /// A day is defined as twenty four hours.
    /// </summary>
    Days = 6
  }

  public sealed class TimeUnitHelper
  {
    /// <summary>
    /// Handy constants for conversion methods.
    /// </summary>
    const long C0 = 1L;
    const long C1 = C0 * 1000L;
    const long C2 = C1 * 1000L;
    const long C3 = C2 * 1000L;
    const long C4 = C3 * 60L;
    const long C5 = C4 * 60L;
    const long C6 = C5 * 24L;

    const long MAX = long.MaxValue;

    /// <summary>
    /// Scale <paramref name="d"/> by <paramref name="m"/>, checking for
    /// overflow. This has a short name to make above code more readable.
    /// </summary>
    static long x(long d, long m, long over) {
      if (d > over) return long.MaxValue;
      if (d < -over) return long.MinValue;
      return d * m;
    }

    /// <summary>
    /// Convert the specified time duration in the given unit to the
    /// destination unit.
    /// </summary>
    /// <returns></returns>
    //public static long Convert(long source_duration, TimeUnit source_unit,
      //TimeUnit dest_unit) {
    //}

    /// <summary>
    /// Convert the specified time duration in the given unit to the
    /// nanoseconds units.
    /// </summary>
    /// <returns></returns>
    public static long ToNanos(long duration, TimeUnit unit) {
      switch (unit) {
        case TimeUnit.Nanoseconds:
          return duration;

        case TimeUnit.Microseconds:
          return x(duration, C1 / C0, MAX / (C1 / C0));

        case TimeUnit.Miliseconds:
          return x(duration, C2 / C0, MAX / (C2 / C0));

        case TimeUnit.Seconds:
          return x(duration, C3 / C0, MAX / (C3 / C0));

        case TimeUnit.Minutes:
          return x(duration, C4 / C0, MAX / (C4 / C0));

        case TimeUnit.Hours:
          return x(duration, C5 / C0, MAX / (C5 / C0));

        case TimeUnit.Days:
          return x(duration, C6 / C0, MAX / (C6 / C0));
      }
      throw new ArgumentOutOfRangeException("unit");
    }

    // TODO: Implement this.
    public static long ToSeconds(long duration, TimeUnit unit) {
      switch (unit) {
        case TimeUnit.Nanoseconds:
          return duration;

        case TimeUnit.Microseconds:
          return x(duration, C1 / C0, MAX / (C1 / C0));

        case TimeUnit.Miliseconds:
          return x(duration, C2 / C0, MAX / (C2 / C0));

        case TimeUnit.Seconds:
          return x(duration, C3 / C0, MAX / (C3 / C0));

        case TimeUnit.Minutes:
          return x(duration, C4 / C0, MAX / (C4 / C0));

        case TimeUnit.Hours:
          return x(duration, C5 / C0, MAX / (C5 / C0));

        case TimeUnit.Days:
          return x(duration, C6 / C0, MAX / (C6 / C0));
      }
      throw new ArgumentOutOfRangeException("unit");
    }
  }
}
