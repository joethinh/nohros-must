using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Nohros.Extensions
{
  public class ProviderOptionsTests
  {
    [Test]
    public void ShouldGetValueAssociatedWithKey() {
      var options = new Dictionary<string, string>();
      options["int-key"] = "40";
      options["long-key"] = "64800";
      options["string-key"] = "64800";
      int i = options.GetInteger("int-key", 30);
      long l = options.GetLong("long-key", 64801);
      string s = options.GetString("string-key", string.Empty);
      Assert.AreEqual(40, i);
      Assert.AreEqual(64800, l);
      Assert.AreEqual("64800", s);

      options.TryGetInteger("int-key", out i);
      options.TryGetLong("long-key", out l);
      Assert.That(i, Is.EqualTo(40));
      Assert.That(l, Is.EqualTo(64800));
    }

    [Test]
    public void ShouldReturnTrueWhenValueAssociatedWithKeyExists() {
      var options = new Dictionary<string, string>();
      options["int-key"] = "40";
      options["long-key"] = "64800";
      options["string-key"] = "64800";
      int i;
      Assert.That(options.TryGetInteger("int-key", out i), Is.True);

      long l;
      Assert.That(options.TryGetLong("long-key", out l), Is.True);
    }

    [Test]
    public void ShouldReturnFalseWhenValueAssociatedWithKeyIsNotFound() {
      var options = new Dictionary<string, string>();
      options["int-key"] = "40";
      options["long-key"] = "64800";
      options["string-key"] = "64800";
      int i;
      Assert.That(options.TryGetInteger("missing-key", out i), Is.False);

      long l;
      Assert.That(options.TryGetLong("missing-key", out l), Is.False);
    }

    [Test]
    public void ShouldReturnFalseWhenValueAssociatedWithKeyIsNotConvertible()
    {
      var options = new Dictionary<string, string>();
      options["int-key"] = "40";
      options["long-key"] = "64800";
      options["string-key"] = "string";
      int i;
      Assert.That(options.TryGetInteger("string-key", out i), Is.False);

      long l;
      Assert.That(options.TryGetLong("string-key", out l), Is.False);
    }

    [Test]
    public void ShouldReturnDefaultValueWhenKeyIsNotFound() {
      var options = new Dictionary<string, string>();
      int i = options.GetInteger("int-key", 30);
      long l = options.GetLong("long-key", 64801);
      string s = options.GetString("string-key", string.Empty);
      Assert.AreEqual(30, i);
      Assert.AreEqual(64801, l);
      Assert.AreEqual(string.Empty, s);
    }
  }
}
