using System;
using NUnit.Framework;

namespace Nohros.Metrics
{
  public class MetricNameTests
  {
    static readonly MetricName metric_name_1_ =
      new MetricName("group", "type", "name");

    static readonly MetricName metric_name_2_ =
      new MetricName("group", "type", "name", "scope");

    static readonly MetricName metric_name_3_
      = new MetricName(typeof (Counter), "name");

    static readonly MetricName metric_name_4_ =
      new MetricName(typeof (Counter), "name", "scope");

    [Test]
    public void ShouldHasGroup() {
      Assert.That(metric_name_1_.Group, Is.EqualTo("group"));
      Assert.That(metric_name_2_.Group, Is.EqualTo("group"));
      Assert.That(metric_name_3_.Group, Is.EqualTo(typeof (Counter).Namespace));
      Assert.That(metric_name_4_.Group, Is.EqualTo(typeof (Counter).Namespace));
    }

    [Test]
    public void ShouldHasType() {
      Assert.That(metric_name_1_.Type, Is.EqualTo("type"));
      Assert.That(metric_name_2_.Type, Is.EqualTo("type"));
      Assert.That(metric_name_3_.Type, Is.EqualTo(typeof (Counter).Name));
      Assert.That(metric_name_4_.Type, Is.EqualTo(typeof (Counter).Name));
    }

    [Test]
    public void ShouldHasName() {
      Assert.That(metric_name_1_.Name, Is.EqualTo("name"));
      Assert.That(metric_name_2_.Name, Is.EqualTo("name"));
      Assert.That(metric_name_3_.Name, Is.EqualTo("name"));
      Assert.That(metric_name_4_.Name, Is.EqualTo("name"));
    }

    [Test]
    public void ShouldHasScope() {
      Assert.That(metric_name_2_.Scope, Is.EqualTo("scope"));
      Assert.That(metric_name_2_.HasScope, Is.EqualTo(true));
      Assert.That(metric_name_4_.Scope, Is.EqualTo("scope"));
      Assert.That(metric_name_4_.HasScope, Is.EqualTo(true));
    }

    [Test]
    public void ShouldNotHaveScope() {
      Assert.That(metric_name_1_.Scope, Is.EqualTo(string.Empty));
      Assert.That(metric_name_1_.HasScope, Is.EqualTo(false));
      Assert.That(metric_name_3_.Scope, Is.EqualTo(string.Empty));
      Assert.That(metric_name_3_.HasScope, Is.EqualTo(false));
    }

    [Test]
    public void ShouldBeHumanReadable() {
      Assert.That(metric_name_1_.ToString(), Is.EqualTo("group.type.name"));
      Assert.That(metric_name_2_.ToString(), Is.EqualTo("group.type.name.scope"));
      Assert.That(metric_name_3_.ToString(),
        Is.EqualTo(typeof (Counter).Namespace + "." + typeof (Counter).Name +
          ".name"));
      Assert.That(metric_name_4_.ToString(),
        Is.EqualTo(typeof (Counter).Namespace + "." + typeof (Counter).Name +
          ".name.scope"));
    }

    [Test]
    public void SameMetricsShouldBeEquals() {
      Assert.That(metric_name_1_, Is.EqualTo(metric_name_1_));
      Assert.That(metric_name_2_, Is.EqualTo(metric_name_2_));
      Assert.That(metric_name_3_, Is.EqualTo(metric_name_3_));
      Assert.That(metric_name_4_, Is.EqualTo(metric_name_4_));
    }

    [Test]
    public void SameMetricsShouldHaveSameHashCode() {
      Assert.That(metric_name_1_.GetHashCode(),
        Is.EqualTo(metric_name_1_.GetHashCode()));

      Assert.That(metric_name_2_.GetHashCode(),
        Is.EqualTo(metric_name_2_.GetHashCode()));

      Assert.That(metric_name_3_.GetHashCode(),
        Is.EqualTo(metric_name_3_.GetHashCode()));

      Assert.That(metric_name_4_.GetHashCode(),
        Is.EqualTo(metric_name_4_.GetHashCode()));
    }
  }
}
