using System;
using NUnit.Framework;

namespace Nohros
{
  [TestFixture]
  public class DateRangeTests
  {
    public void enumerate_all_days_in_range(int granularity) {
      var first_date = new DateTime(2014, 03, 10);
      var last_date = new DateTime(2014, 04, 10);
      var range = new DateRange(first_date, last_date, granularity);

      DateTime current = first_date;
      foreach (DateTime date in range) {
        Assert.That(date, Is.EqualTo(current));
        current = current.AddDays(granularity);
      }
    }

    [Test]
    public void should_enumerate_all_days_in_range() {
      enumerate_all_days_in_range(1);
    }

    [Test]
    public void should_enumerate_all_months_in_range() {
      var first_date = new DateTime(2014, 01, 01);
      var last_date = new DateTime(2014, 12, 01);

      var range = new DateRange(first_date, last_date, 1,
        DateGranularityUnit.Month);

      DateTime current = first_date;
      foreach (DateTime date in range) {
        Assert.That(date, Is.EqualTo(current));
        current = current.AddMonths(1);
      }
    }

    [Test]
    public void should_enumerate_all_quarters_in_range() {
      enumerate_all_days_in_range(15);
    }

    [Test]
    public void should_enumerate_all_weeks_in_range() {
      enumerate_all_days_in_range(7);
    }

    [Test]
    public void should_not_accept_negative_granularity() {
      Assert.Throws<ArgumentOutOfRangeException>(
        () => new DateRange(DateTime.Now, DateTime.Now, -10));

      Assert.Throws<ArgumentOutOfRangeException>(
        () =>
          new DateRange(DateTime.Now, DateTime.Now, -10,
            DateGranularityUnit.Day));

      Assert.Throws<ArgumentOutOfRangeException>(
        () =>
          new DateRange(DateTime.Now, DateTime.Now, -10,
            DateGranularityUnit.Month));

      Assert.Throws<ArgumentOutOfRangeException>(
        () =>
          new DateRange(DateTime.Now, DateTime.Now, -10,
            DateGranularityUnit.Year));
    }

    [Test]
    public void should_stop_enumeration_on_last_date() {
      var first_date = new DateTime(2014, 03, 10);
      var last_date = new DateTime(2014, 04, 10);
      var range = new DateRange(first_date, last_date);

      DateTime current = first_date;
      foreach (DateTime date in range) {
        current = date;
      }

      Assert.That(current, Is.EqualTo(last_date));
    }

    [Test]
    public void should_use_day_as_default_granularity_unit() {
      var range = new DateRange();
      Assert.That(range.GranularityUnit, Is.EqualTo(DateGranularityUnit.Day));

      range = new DateRange(DateTime.Now, DateTime.Now);
      Assert.That(range.GranularityUnit, Is.EqualTo(DateGranularityUnit.Day));
    }

    [Test]
    public void should_use_one_as_default_granularity() {
      var range = new DateRange();
      Assert.That(range.Granularity, Is.EqualTo(1));

      range = new DateRange(DateTime.Now, DateTime.Now);
      Assert.That(range.Granularity, Is.EqualTo(1));
    }
  }
}
