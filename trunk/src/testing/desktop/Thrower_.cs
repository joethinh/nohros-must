using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Nohros.Test.Common
{
    [TestFixture]
    public class Thrower_
    {
        [Test]
        public void ThrowArgumentException() {
            Assert.Throws<ArgumentException>(delegate() {
                Thrower.ThrowArgumentException(ExceptionResource.Argument_Empty);
            });
        }

        [Test]
        public void ThrowArgumentNullException() {
            Assert.Throws<ArgumentNullException>(delegate() {
                Thrower.ThrowArgumentNullException(ExceptionArgument.any);
            });
        }

        [Test]
        public void ThrowArgumentOutOfRangeException() {
            Assert.Throws<ArgumentOutOfRangeException>(delegate() {
                Thrower.ThrowArgumentOutOfRangeException(ExceptionArgument.any);
            });
        }

        [Test]
        public void ThrowEmptyArgumentException() {
            Assert.Throws<ArgumentException>(delegate() {
                Thrower.ThrowEmptyArgumentException(ExceptionArgument.any);
            });
        }

        [Test]
        public void ThrowInvalidOperationException() {
            Assert.Throws<InvalidOperationException>(delegate() {
                Thrower.ThrowInvalidOperationException(ExceptionResource.Argument_Empty);
            });
        }

        [Test]
        public void ThrowKeyNotFoundException() {
            Assert.Throws<KeyNotFoundException>(delegate() {
                Thrower.ThrowKeyNotFoundException();
            });
        }

        [Test]
        public void ThrowProviderException() {
            Assert.Throws<ProviderException>(delegate() {
                Thrower.ThrowProviderException(ExceptionResource.DataProvider_ConnectionString);
            });
        }
    }
}