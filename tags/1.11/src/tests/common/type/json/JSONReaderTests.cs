using System;

using NUnit.Framework;

namespace Nohros.Data
{
  [TestFixture]
  public class JSONReaderTests
  {
    [Test]
    public void Reading() {
      // some whitespace checking.
      IValue root;
      JSONReader reader = new JSONReader();
      root = reader.JsonToValue("   null   ", false, false);
      Assert.True(root.IsType(ValueType.NullValue));
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void InvalidJSONString() {
      JSONReader reader = new JSONReader();
      IValue root = reader.JsonToValue("nu", false, false);
    }

    [Test]
    public void SimpleBool() {
      JSONReader reader = new JSONReader();
      IValue root = reader.JsonToValue("true   ", false, false);
      Assert.True(root.IsType(ValueType.Boolean));
    }

    [Test]
    public void EmbeddedComment() {
      JSONReader reader= new JSONReader();
      IValue root = reader.JsonToValue("/* comment */null", false, false);
      Assert.True(root.IsType(ValueType.NullValue));

      root = reader.JsonToValue("40 /* comment */", false, false);
      Assert.True(root.IsType(ValueType.Integer));

      root = reader.JsonToValue("true // comment", false, false);
      Assert.True(root.IsType(ValueType.Boolean));

      root = reader.JsonToValue("/* comment */\"sample string\"", false, false);
      Assert.True(root.IsType(ValueType.String));
      Assert.AreEqual("sample string", root.GetAsString());
    }

    [Test]
    public void NumberFormat() {
      JSONReader reader = new JSONReader();
      IValue root = reader.JsonToValue("43", false, false);
      Assert.True(root.IsType(ValueType.Integer));
      Assert.AreEqual(43, root.GetAsInteger());
    }
  }
}
