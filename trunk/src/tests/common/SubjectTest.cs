using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace Nohros.Security.Auth
{
    [TestFixture]
    public class SubjectTest
    {
        [Test]
        public void ShouldConstructAEmptySubject() {
            Subject subject = new Subject();
            Assert.IsNotNull(subject.Permissions);
            Assert.IsNotNull(subject.Principals);
            Assert.AreEqual(0, subject.Permissions.Count);
            Assert.AreEqual(0, subject.Principals.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionWhenPermissionSetIsNull() {
            Subject subject = new Subject((PermissionSet)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionWhenPrincipalSetIsNull() {
            Subject subject = new Subject((PrincipalSet)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionWhenSetsAreNull() {
            Subject subject = new Subject((PrincipalSet)null,(PermissionSet)principals);
        }
    }
}
