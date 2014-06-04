using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using NUnit.Framework;

namespace Nohros.Test.Common
{
  [TestFixture]
  public class Thrower_
  {
    [Test]
    [ExpectedExceptionAttribute(typeof (ArgumentException))]
    public void ThrowArgumentException() {
      Thrower.ThrowArgumentException(ExceptionResource.Argument_Empty);
    }

    [Test]
    [ExpectedExceptionAttribute(typeof (ArgumentNullException))]
    public void ThrowArgumentNullException() {
      Thrower.ThrowArgumentNullException(ExceptionArgument.any);
    }

    [Test]
    [ExpectedExceptionAttribute(typeof (ArgumentOutOfRangeException))]
    public void ThrowArgumentOutOfRangeException() {
      Thrower.ThrowArgumentOutOfRangeException(ExceptionArgument.any);
    }

    [Test]
    [ExpectedExceptionAttribute(typeof (ConfigurationErrorsException))]
    public void ThrowConfigurationException() {
      Thrower.ThrowConfigurationException("message",
        "[Nohros.Test.Common.Thrower]");
    }

    [Test]
    [ExpectedExceptionAttribute(typeof (ArgumentException))]
    public void ThrowEmptyArgumentException() {
      Thrower.ThrowEmptyArgumentException(ExceptionArgument.any);
    }

    [Test]
    [ExpectedExceptionAttribute(typeof (InvalidOperationException))]
    public void ThrowInvalidOperationException() {
      Thrower.ThrowInvalidOperationException(ExceptionResource.Argument_Empty);
    }
  }
}
