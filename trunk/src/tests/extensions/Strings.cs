using System;
using NUnit.Framework;

namespace Nohros.Extensions
{
  public class StringsTests
  {
    [Test]
    public void ShouldReturnTrueWhenTwoStringsAreOrdinaryEquals() {
      Assert.That("first".CompareOrdinal("first"), Is.True);
    }

    [Test]
    public void ShouldReturnFalseWhenTwoStringsAreNotOrdinaryEquals() {
      Assert.That("first".CompareOrdinal("second"), Is.False);
      Assert.That("first".CompareOrdinal("First"), Is.False);
      Assert.That("first".CompareOrdinal("firsT"), Is.False);
      Assert.That("first".CompareOrdinal("fIrSt"), Is.False);
    }

    [Test]
    public void ShouldReturnFalseWhenStringIsNullOrEmpty() {
      Assert.That(string.Empty.IsNullOrEmpty(), Is.False);
      Assert.That(((string) null).IsNullOrEmpty(), Is.True);
    }
  }
}
