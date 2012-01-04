using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace Nohros.Security.Auth
{
    [TestFixture]
    public class PrincipalTest
    {
        [Test]
        public void PrincipalsWithTheSameNameMustBeEquals() {
            Principal principal = new Principal("someprincipal");
            Principal principal_2 = new Principal("someprincipal");
            Assert.IsTrue(principal.Equals(principal_2));
            Assert.IsTrue(principal_2.Equals(principal));
        }

        [Test]
        public void PrincipalsWithNonEqualsNameMustBeNonEquals() {
            Principal principal = new Principal("someprincipal");
            Principal principal_2 = new Principal("otherprincipal");
            Assert.IsFalse(principal.Equals(principal_2));
            Assert.IsFalse(principal_2.Equals(principal));
        }

        [Test]
        public void EqualsPrincipalsMustHaveTheSameHashCode() {
            Principal principal = new Principal("someprincipal");
            Principal principal_2 = new Principal("someprincipal");
            Assert.AreEqual(principal.GetHashCode(), principal_2.GetHashCode());
        }

        [Test]
        public void NonEqualsPrincipalsMustHaveDifferentHashCode() {
            Principal principal = new Principal("someprincipal");
            Principal principal_2 = new Principal("otherprincipal");
            Assert.AreNotEqual(principal.GetHashCode(), principal_2.GetHashCode());
        }
    }
}
