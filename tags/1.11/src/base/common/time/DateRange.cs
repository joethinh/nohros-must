using System;
using System.Collections;
using System.Collections.Generic;

namespace Nohros
{
  /// <summary>
  /// Defines the small unit of a date range.
  /// </summary>
  public enum DateGranularityUnit
  {
    /// <summary>
    /// Represents a day.
    /// </summary>
    Day = 0,

    /// <summary>
    /// Represents a month.
    /// </summary>
    Month = 1,

    /// <summary>
    /// Represents a year.
    /// </summary>
    Year = 2
  }

  /// <summary>
  /// Represents a range of dates.
  /// </summary>
  public struct DateRange : IEnumerable<DateTime>
  {
    readonly DateTime first_date_;
    readonly int granularity_;
    readonly DateGranularityUnit granularity_unit_;
    readonly DateTime last_date_;

    /// <summary>
    /// Initializes the <see cref="DateRange"/> by using the given first and
    /// last dates of the range.
    /// </summary>
    /// <param name="first_date">
    /// The first date of the range.
    /// </param>
    /// <param name="last_date">
    /// The last date if the range
    /// </param>
    /// <remarks>
    /// The dates of the range is enumerated using one unit of
    /// <see cref="GranularityUnit.Day"/>.
    /// </remarks>
    public DateRange(DateTime first_date, DateTime last_date)
      : this(first_date, last_date, 1, DateGranularityUnit.Day) {
    }

    /// <summary>
    /// Initializes the <see cref="DateRange"/> by using the given first and
    /// last dates of the range.
    /// </summary>
    /// <param name="first_date">
    /// The first date of the range.
    /// </param>
    /// <param name="last_date">
    /// The last date if the range
    /// </param>
    /// <param name="granularity">
    /// The granularity of the step. This represents the number of steps to
    /// advance per item enumerated while enumerating the dates of a range.
    /// </param>
    /// <remarks>
    /// The dates of the range is enumerated using
    /// <paramref name="granularity"/> units of
    /// <see cref="GranularityUnit.Day"/>.
    /// </remarks>
    public DateRange(DateTime first_date, DateTime last_date, int granularity)
      : this(first_date, last_date, granularity, DateGranularityUnit.Day) {
    }

    /// <summary>
    /// Initializes the <see cref="DateRange"/> by using the given first,
    /// step type and number of steps per unit.
    /// </summary>
    /// <param name="first_date">
    /// The first date of the range.
    /// </param>
    /// <param name="last_date">
    /// The last date if the range
    /// </param>
    /// <param name="granularity">
    /// The granularity of the step. This represents the number of steps to
    /// advance per item enumerated while enumerating the dates of a range.
    /// </param>
    /// <param name="granularity_unit">
    /// The type of the <paramref name="granularity"/>.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="first_date"/> is greater than
    /// <paramref name="last_date"/> - or -
    /// <para>
    /// <paramref name="granularity"/> is negative - or -
    /// </para>
    /// </exception>
    /// <remarks>
    /// The dates of the range is enumerated using
    /// <paramref name="granularity"/> units of
    /// <paramref name="granularity_unit"/>.
    /// </remarks>
    public DateRange(DateTime first_date, DateTime last_date, int granularity,
      DateGranularityUnit granularity_unit) {
      if (first_date > last_date) {
        throw new ArgumentOutOfRangeException("first_date",
          "The first date should be greater than the last date of the range");
      }

      if (granularity < 0) {
        throw new ArgumentOutOfRangeException("granularity",
          "The granularity should not be negative");
      }

      first_date_ = first_date;
      last_date_ = last_date;
      granularity_ = granularity;
      granularity_unit_ = granularity_unit;
    }

    /// <summary>
    /// Gets a <see cref="IEnumerator"/> that enumerates the dates of a
    /// range.
    /// </summary>
    /// <returns>
    /// A <see cref="IEnumerator"/> that enumerates the dates of a range.
    /// </returns>
    /// <remarks>
    /// The enumeration starts at <see cref="FirstDate"/> and increments one
    /// <see cref="Step"/> at each enumeration.
    /// </remarks>
    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    /// <summary>
    /// Gets a <see cref="IEnumerator"/> that enumerates the dates of a
    /// range.
    /// </summary>
    /// <returns>
    /// A <see cref="IEnumerator"/> that enumerates the dates of a range.
    /// </returns>
    /// <remarks>
    /// The enumeration starts at <see cref="FirstDate"/> and increments one
    /// <see cref="Granularity"/> at each enumeration.
    /// </remarks>
    public IEnumerator<DateTime> GetEnumerator() {
      switch (granularity_unit_) {
        case DateGranularityUnit.Day:
          return GetEnumeratorForDay();
        case DateGranularityUnit.Month:
          return GetEnumeratorForMonth();
        case DateGranularityUnit.Year:
          return GetEnumeratorForYear();
        default:
          throw new NotReachedException();
      }
    }

    IEnumerator<DateTime> GetEnumeratorForDay() {
      DateTime current = first_date_;
      while (current <= last_date_) {
        yield return current;
        current = current.AddDays(Granularity);
      }
    }

    IEnumerator<DateTime> GetEnumeratorForYear() {
      DateTime current = first_date_;
      while (current <= last_date_) {
        yield return current;
        current = current.AddYears(Granularity);
      }
    }

    IEnumerator<DateTime> GetEnumeratorForMonth() {
      DateTime current = first_date_;
      while (current <= last_date_) {
        yield return current;
        current = current.AddMonths(Granularity);
      }
    }

    /// <summary>
    /// Gets the first date of the range.
    /// </summary>
    public DateTime FirstDate {
      get { return first_date_; }
    }

    /// <summary>
    /// Gets the last date of the range.
    /// </summary>
    public DateTime LastDate {
      get { return last_date_; }
    }

    /// <summary>
    /// Gets the unit of the granularity.
    /// </summary>
    public DateGranularityUnit GranularityUnit {
      get { return granularity_unit_; }
    }

    /// <summary>
    /// Gets the granularity that is used to enumerate the dates of a range.
    /// </summary>
    public int Granularity {
      get { return (granularity_ < 1) ? 1 : granularity_; }
    }
  }
}
