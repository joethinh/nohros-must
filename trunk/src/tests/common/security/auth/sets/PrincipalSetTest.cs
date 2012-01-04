using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Nohros.Security.Auth;

namespace Nohros.Security.Auth
{
    [TestFixture]
    public class PrincipalSetTest
    {
        [Test]
        public void ShouldCreateAEmptySet() {
            PrincipalSet set = new PrincipalSet();
            Assert.AreEqual(0, set.Count);
        }

        [Test]
        public void ShouldCreateASetWithTheGivenElements() {
            List<IPrincipal> principals = new List<IPrincipal>(4);
            principals.Add((IPrincipal)new Principal("principal1"));
            principals.Add((IPrincipal)new Principal("principal2"));
            principals.Add((IPrincipal)new Principal("principal4"));
            principals.Add((IPrincipal)new Principal("principal3"));

            PrincipalSet set = new PrincipalSet(principals);
            Assert.AreEqual(4, set.Count);
            Assert.IsTrue(set.Contains((IPrincipal)new Principal("principal1")));
            Assert.IsTrue(set.Contains((IPrincipal)new Principal("principal4")));
            Assert.IsTrue(set.Contains((IPrincipal)new Principal("principal3")));
            Assert.IsTrue(set.Contains((IPrincipal)new Principal("principal2")));
        }

        [Test]
        public void ShouldAdjustInternalArrayWhileElementsAreAddedAndRemoved() {
            PrincipalSet set = new PrincipalSet();
            set.Add(new Principal("someprincipal"));
            Assert.AreEqual(1, set.Count);
            set.Add(new Principal("anotherprincipal"));
            Assert.AreEqual(2, set.Count);
            set.Remove(new Principal("someprincipal"));
            Assert.AreEqual(1, set.Count);
        }

        [Test]
        public void ShouldNotAddDuplicates() {
            PrincipalSet set = new PrincipalSet();
            set.Add(new Principal("someprincipal"));
            Assert.AreEqual(1, set.Count);
            set.Add(new Principal("someprincipal"));
            Assert.AreEqual(1, set.Count);
        }

        [Test]
        public void ShouldReturnFalseWhenAddingAnExistingElement() {
            PrincipalSet set = new PrincipalSet();
            set.Add(new Principal("someprincipal"));
            Assert.IsFalse(set.Add(new Principal("someprincipal")));
        }

        [Test]
        public void ShouldReturnTrueWhenAddingAnNewElement() {
            PrincipalSet set = new PrincipalSet();
            Assert.IsTrue(set.Add(new Principal("someprincipal")));
        }

        [Test]
        public void ShouldRemoveReferencesToElementsInSetAfterClear() {
            PrincipalSet set = new PrincipalSet();
            Principal principal = new Principal("someprincipal");
            set.Add(principal);
            set.Clear();
            Assert.AreEqual(0, set.Count);
            Assert.IsTrue(set.Add(principal));
        }

        [Test]
        public void ShouldReturnTrueWhenAnElementExistInSet() {
            PrincipalSet set = new PrincipalSet();
            Principal principal = new Principal("someprincipal");
            set.Add(principal);
            Assert.IsTrue(set.Contains(principal));
        }

        [Test]
        public void ShouldReturnTrueWhenAnElementDoesNotExistInSet() {
            PrincipalSet set = new PrincipalSet();
            Principal principal = new Principal("someprincipal");
            set.Add(principal);
            Assert.IsFalse(set.Contains(new Principal("otherprincipal")));
        }

        [Test]
        public void ShouldReturnTrueWhenAnExistingElementIsRemoved() {
            PrincipalSet set = new PrincipalSet();
            Principal principal = new Principal("someprincipal");
            set.Add(principal);
            Assert.IsTrue(set.Remove(principal));
        }

        [Test]
        public void ShouldReturnFalseWhenAttemptToRemoveAnElementNotPresentInSet() {
            PrincipalSet set = new PrincipalSet();
            Principal principal = new Principal("someprincipal");
            set.Add(principal);
            Assert.IsFalse(set.Remove(new Principal("otherprincipal")));
        }

        [Test]
        public void ShouldNotThrowAnExceptionWhenAElementToRemoveIsNull() {
            PrincipalSet set = new PrincipalSet();
            Principal principal = new Principal("someprincipal");
            set.Add(principal);
            set.Remove(null);
            Assert.Pass();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionWhenAnElementToAddIsNull() {
            PrincipalSet set = new PrincipalSet();
            Principal principal = new Principal("someprincipal");
            set.Add(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ShouldThrowArgumentOutOfRangeExceptionWhenCapacityIsLessThanZero() {
            PrincipalSet set = new PrincipalSet(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionWhenElementsColectionIsNull() {
            PrincipalSet set = new PrincipalSet((IEnumerable<IPrincipal>)null);
        }
    }
}
