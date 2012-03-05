using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Nohros.Concurrent.Tests
{
  [TestFixture]
  public class AtomicReferenceTests
  {
    public class ReferenceMock {
      public ReferenceMock(string value) {
        Value = value;
      }
      public string Value;
    }

    public ReferenceMock Reference {
      get { return new ReferenceMock("reference-mock"); }
    }

    [Test]
    public void ShouldUpdateWhenComparisonIsTrueAndReturnOldValue() {
      ReferenceMock reference = Reference;

      AtomicReference<ReferenceMock> ar =
        new AtomicReference<ReferenceMock>(reference);

      ReferenceMock new_mock = new ReferenceMock("new-mock");
      ReferenceMock old_mock = ar.CompareExchange(reference, new_mock);
      Assert.AreEqual(new_mock, ar.Value);
      Assert.AreEqual(reference, old_mock);
    }

    [Test]
    public void ShouldNotUpdateWhenComparisonIsFalseAndReturnOldValue() {
      ReferenceMock reference = Reference;

      AtomicReference<ReferenceMock> ar =
        new AtomicReference<ReferenceMock>(reference);

      ReferenceMock new_mock = new ReferenceMock("new-mock");
      ReferenceMock old_mock = ar.CompareExchange(new_mock, new_mock);
      Assert.AreEqual(reference, ar.Value);
      Assert.AreEqual(reference, old_mock);
    }

    [Test]
    public void ShouldExchangeAndReturnOldValue() {
      ReferenceMock reference = Reference;

      AtomicReference<ReferenceMock> ar =
        new AtomicReference<ReferenceMock>(reference);

      ReferenceMock new_mock = new ReferenceMock("new_mock");
      ReferenceMock old_mock = ar.Exchange(new_mock);
      Assert.AreEqual(new_mock, ar.Value);
      Assert.AreEqual(reference, old_mock);
    }
  }
}
