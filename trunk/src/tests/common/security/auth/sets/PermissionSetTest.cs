using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Nohros.Security.Auth;

namespace Nohros.Security.Auth
{
    [TestFixture]
    public class PermissionSetTest
    {
        [Test]
        public void ShouldCreateAEmptySet() {
            PermissionSet set = new PermissionSet();
            Assert.AreEqual(0, set.Count);
        }

        [Test]
        public void ShouldCreateASetWithTheGivenElements() {
            List<IPermission> permissions = new List<IPermission>(4);
            permissions.Add((IPermission)new BasicPermission("permission1"));
            permissions.Add((IPermission)new BasicPermission("permission2"));
            permissions.Add((IPermission)new BasicPermission("permission4"));
            permissions.Add((IPermission)new BasicPermission("permission3"));

            PermissionSet set = new PermissionSet(permissions);
            Assert.AreEqual(4, set.Count);
            Assert.IsTrue(set.Contains((IPermission)new BasicPermission("permission1")));
            Assert.IsTrue(set.Contains((IPermission)new BasicPermission("permission4")));
            Assert.IsTrue(set.Contains((IPermission)new BasicPermission("permission3")));
            Assert.IsTrue(set.Contains((IPermission)new BasicPermission("permission2")));
        }

        [Test]
        public void ShouldAdjustInternalArrayWhileElementsAreAddedAndRemoved() {
            PermissionSet set = new PermissionSet();
            set.Add(new BasicPermission("somepermission"));
            Assert.AreEqual(1, set.Count);
            set.Add(new BasicPermission("anotherpermission"));
            Assert.AreEqual(2, set.Count);
            set.Remove(new BasicPermission("somepermission"));
            Assert.AreEqual(1, set.Count);
        }

        [Test]
        public void ShouldNotAddDuplicates() {
            PermissionSet set = new PermissionSet();
            set.Add(new BasicPermission("somepermission"));
            Assert.AreEqual(1, set.Count);
            set.Add(new BasicPermission("somepermission"));
            Assert.AreEqual(1, set.Count);
        }

        [Test]
        public void ShouldReturnFalseWhenAddingAnExistingElement() {
            PermissionSet set = new PermissionSet();
            set.Add(new BasicPermission("somepermission"));
            Assert.IsFalse(set.Add(new BasicPermission("somepermission")));
        }

        [Test]
        public void ShouldReturnTrueWhenAddingAnNewElement() {
            PermissionSet set = new PermissionSet();
            Assert.IsTrue(set.Add(new BasicPermission("somepermission")));
        }

        [Test]
        public void ShouldRemoveReferencesToElementsInSetAfterClear() {
            PermissionSet set = new PermissionSet();
            BasicPermission permission = new BasicPermission("somepermission");
            set.Add(permission);
            set.Clear();
            Assert.AreEqual(0, set.Count);
            Assert.IsTrue(set.Add(permission));
        }

        [Test]
        public void ShouldReturnTrueWhenAnElementExistInSet() {
            PermissionSet set = new PermissionSet();
            BasicPermission permission = new BasicPermission("somepermission");
            set.Add(permission);
            Assert.IsTrue(set.Contains(permission));
        }

        [Test]
        public void ShouldReturnTrueWhenAnElementDoesNotExistInSet() {
            PermissionSet set = new PermissionSet();
            BasicPermission permission = new BasicPermission("somepermission");
            set.Add(permission);
            Assert.IsFalse(set.Contains(new BasicPermission("otherpermission")));
        }

        [Test]
        public void ShouldReturnTrueWhenAnExistingElementIsRemoved() {
            PermissionSet set = new PermissionSet();
            BasicPermission permission = new BasicPermission("somepermission");
            set.Add(permission);
            Assert.IsTrue(set.Remove(permission));
        }

        [Test]
        public void ShouldReturnFalseWhenAttemptToRemoveAnElementNotPresentInSet() {
            PermissionSet set = new PermissionSet();
            BasicPermission permission = new BasicPermission("somepermission");
            set.Add(permission);
            Assert.IsFalse(set.Remove(new BasicPermission("otherpermission")));
        }

        [Test]
        public void ShouldNotThrowAnExceptionWhenAElementToRemoveIsNull() {
            PermissionSet set = new PermissionSet();
            BasicPermission permission = new BasicPermission("somepermission");
            set.Add(permission);
            set.Remove(null);
            Assert.Pass();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionWhenAnElementToAddIsNull() {
            PermissionSet set = new PermissionSet();
            BasicPermission permission = new BasicPermission("somepermission");
            set.Add(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ShouldThrowArgumentOutOfRangeExceptionWhenCapacityIsLessThanZero() {
            PermissionSet set = new PermissionSet(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionWhenElementsColectionIsNull() {
            PermissionSet set = new PermissionSet((IEnumerable<IPermission>)null);
        }
    }
}
