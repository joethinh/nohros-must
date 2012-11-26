using System;
using NUnit.Framework;

namespace Nohros.Concurrent
{
  public class AbstractFutureTests
  {
    public class AbstractFutureMock : AbstractFuture<string>
    {
      public new bool Set(string value) {
        return base.Set(value, false);
      }

      public new bool SetException(Exception exception) {
        return base.SetException(exception, false);
      }
    }

    [Test]
    public void ShouldGetSetValue() {
      var mock = new AbstractFutureMock();
      mock.Set("somevalue");
      Assert.That(mock.Get(), Is.EqualTo("somevalue"));
    }

    [Test]
    public void ShouldThrowSetException() {
      var mock = new AbstractFutureMock();
      mock.SetException(new ArgumentException());
      Assert.That(() => mock.Get(),
        Throws.InnerException.TypeOf<ArgumentException>());
    }
  }
}
