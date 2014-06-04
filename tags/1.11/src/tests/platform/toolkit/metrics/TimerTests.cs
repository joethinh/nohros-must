using System;
using NUnit.Framework;

namespace Nohros.Metrics
{
  public class TimerTests
  {
    class ClockMock : Clock
    {
      long val_ = 0;

      public override long Tick {
        get { return val_ += 50000000; }
      }
    }

    [Test]
    public void ShouldHasDurationUnit() {
      var timer = Timer;
      Assert.That(timer.DurationUnit, Is.EqualTo(TimeUnit.Milliseconds));
    }

    [Test]
    public void ShouldCreateEmptyTimer() {
      var timer = Timer;
      Assert.That(timer.Count, Is.EqualTo(0));
      Assert.That(timer.Max, Is.InRange(0, 0.0001));
      Assert.That(timer.Min, Is.InRange(0, 0.0001));
      Assert.That(timer.Mean, Is.InRange(0, 0.0001));
      Assert.That(timer.StandardDeviation, Is.InRange(0, 0.0001));

      Snapshot snapshot = timer.Snapshot;
      Assert.That(snapshot.Median, Is.InRange(0, 0.0001));
      Assert.That(snapshot.Percentile75, Is.InRange(0, 0.0001));
      Assert.That(snapshot.Percentile99, Is.InRange(0, 0.0001));
      Assert.That(snapshot.Size, Is.EqualTo(0));
    }

    [Test]
    public void ShouldTimeSeriesOfEvents() {
      var timer = new Timer(TimeUnit.Milliseconds,
        new Meter("calls", TimeUnit.Seconds), Histograms.Biased(),
        new ClockMock());

      timer.Update(10, TimeUnit.Milliseconds);
      timer.Update(20, TimeUnit.Milliseconds);
      timer.Update(20, TimeUnit.Milliseconds);
      timer.Update(30, TimeUnit.Milliseconds);
      timer.Update(40, TimeUnit.Milliseconds);

      Assert.That(timer.Count, Is.EqualTo(5));
      Assert.That(timer.Max, Is.InRange(40.0, 40.0001));
      Assert.That(timer.Min, Is.InRange(10, 10.0001));
      Assert.That(timer.Mean, Is.InRange(24, 24.0001));
      Assert.That(timer.StandardDeviation, Is.InRange(11.401, 11.402));

      Snapshot snapshot = timer.Snapshot;
      Assert.That(snapshot.Median, Is.InRange(20, 20.0001));
      Assert.That(snapshot.Percentile75, Is.InRange(35, 35.0001));
      Assert.That(snapshot.Percentile99, Is.InRange(40, 40.0001));
      Assert.That(snapshot.Values, Is.EqualTo(new double[] {10, 20, 20, 30, 40}));
    }

    [Test]
    public void ShoulTimeVariantValues() {
      var timer = Timer;
      timer.Update(long.MaxValue, TimeUnit.Nanoseconds);
      timer.Update(0, TimeUnit.Nanoseconds);

      Assert.That(timer.StandardDeviation, Is.InRange(6.521e12, 6.522e12));
    }

    [Test]
    public void ShouldTimeCallableInstances() {
      var timer = Timer;
      string value = timer.Time(() => "one");

      Assert.That(timer.Count, Is.EqualTo(1), "the timer has count of 1");
      Assert.That(value, Is.EqualTo("one"), "returns the result of callable");
      Assert.That(timer.Max, Is.InRange(50.0, 50.001),
        "records the duration of the method call");
    }

    [Test]
    public void ShouldTimeThroughContext() {
      var timer = Timer;
      timer.Time().Stop();

      Assert.That(timer.Count, Is.EqualTo(1), "the timer has count of 1");
      Assert.That(timer.Max, Is.InRange(50.0, 50.001),
        "record the duration of the context");
    }

    public Timer Timer {
      get {
        return new Timer(TimeUnit.Milliseconds,
          new Meter("calls", TimeUnit.Seconds), Histograms.Biased(),
          new ClockMock());
      }
    }
  }
}
