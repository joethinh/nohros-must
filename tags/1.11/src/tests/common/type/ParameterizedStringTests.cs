using System;

using NUnit.Framework;

namespace Nohros
{
  [TestFixture]
  public class ParameterizedStringTests
  {
    [Test]
    public void ShouldThrowArgumentNullExceptionWhenArgIsNull() {
      Assert.Throws<ArgumentNullException>(delegate() {
        ParameterizedString s = new ParameterizedString(null);
      });

      Assert.Throws<ArgumentNullException>(delegate() {
        ParameterizedString s = new ParameterizedString(null, "delimiter");
      });

      Assert.Throws<ArgumentNullException>(delegate() {
        ParameterizedString s = new ParameterizedString("string", null);
      });
    }

    [Test]
    public void ShouldThrowArgumentExceptionWhenOneOrMoreElementsAreNull() {
      Assert.Throws<ArgumentException>(delegate()
      {
        ParameterizedStringPart[] parts = new ParameterizedStringPart[2];
        parts[0] = new ParameterizedStringPartParameter("name");
        parts[1] = null;

        ParameterizedString s =
          ParameterizedString.FromParameterizedStringPartCollection(parts, "$");
      });
    }

    [Test]
    public void ShouldConsiderOnlyDelimiterEnclosedStringsAsParameters() {
      ParameterizedString s =
        new ParameterizedString(
          "This is a parameter $parameter$ and this is not $parameter");
      s.Parse();

      Assert.AreEqual(1, s.Parameters.Count);
      Assert.AreEqual("parameter", s.Parameters[0].Name);
      Assert.AreEqual("This is a parameter  and this is not $parameter",
        s.ToString());

      s = new ParameterizedString(
          "Spaces is part of the parameter: $parameter with spaces$");
      s.Parse();

      Assert.AreEqual(1, s.Parameters.Count);
      Assert.AreEqual("parameter with spaces", s.Parameters[0].Name);
    }

    [Test]
    public void ShouldUseSpaceAsTerminatorWhenSpecified() {
      ParameterizedString s =
        new ParameterizedString(
          "This is a $parameter$ and this is the same $parameter again.");
      s.UseSpaceAsTerminator = true;
      s.Parse();

      Assert.AreEqual(1, s.Parameters.Count);
      Assert.AreEqual("parameter", s.Parameters[0].Name);
      Assert.AreEqual("This is a  and this is the same  again.", s.ToString());

      s =
        new ParameterizedString(
          "This is a $parameter$ and this is the another $parameter2");
      s.UseSpaceAsTerminator = true;
      s.Parse();

      Assert.AreEqual(2, s.Parameters.Count);
      Assert.AreNotEqual(-1, s.Parameters.IndexOf("parameter"));
      Assert.AreNotEqual(-1, s.Parameters.IndexOf("parameter2"));
      Assert.AreEqual("This is a  and this is the another ", s.ToString());
    }

    [Test]
    public void SholdReplaceTheParameterToItsValueOnFinalString() {
      ParameterizedString s =
        new ParameterizedString("The parameter value is $parameter$");
      Assert.AreEqual(string.Empty, s.ToString());

      s.Parse();
      s.Parameters[0].Value = "VALUE";
      Assert.AreEqual("The parameter value is VALUE", s.ToString());

      s = new ParameterizedString(
        "The parameter value is $parameter$ and more.");

      s.Parse();
      s.Parameters[0].Value = "VALUE";
      Assert.AreEqual("The parameter value is VALUE and more.", s.ToString());
    }
  }
}
