using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Represent a set of permissions.
    /// </summary>
    public class PermissionSet : ISecureSet<IPermission>, IEnumerable<IPermission>, IEnumerable
    {
        readonly SecureSet<IPermission> permissions_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionSet"/> class that is empty.
        /// </summary>
        public PermissionSet() {
            permissions_ = new SecureSet<IPermission>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionSet"/> class that contains elements
        /// copied from the specified collection, and has sufficient capacity to accomodate the number of
        /// elements copied.
        /// </summary>
        /// <param name="permissions">The collection whose elements are copied to the new set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="permissions"/> is a null reference.</exception>
        /// <remarks>
        /// The capacity of a <see cref="PermissionSet"/> object is the number of elements that object
        /// can hold. A <see cref="PermissionSet"/> object's capacity automatically increases as elements
        /// are added to the object.
        /// <para>
        /// If the specified collection of permissions contains duplicates, the set will contain one of
        /// each unique element. No exception will be throw. Therefore, the size of the resulting set is
        /// not identical to the size of <paramref name="permissions"/>. Note that the value returned
        /// from the GetHashCode() method of the permissions objects is used when comparing values in the
        /// set. If a element of the specified permissions collection is null a exception will not be
        /// throw and the element will not be stored in the set.
        /// </para>
        /// <para>
        /// This method performs an O(n) operation.
        /// </para>
        /// </remarks>
        public PermissionSet(IEnumerable<IPermission> permissions) {
            permissions_ = new SecureSet<IPermission>(permissions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionSet"/> class that is empty
        /// and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements that this set can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than zero.</exception>
        /// <remarks>
        /// The capacity of a <see cref="PermissionSet"/> object is the number of elements that object can
        /// hold before resizing is necessary. A <see cref="PermissionSet"/> object's capacity automatically
        /// increases as elements are added to the object.
        /// <para>
        /// If the size of the collection can be estimated, specifying the initial capacity eliminates
        /// the need to perform a number of resizing operations while adding elements to the
        /// <see cref="PermissionSet"/>
        /// </para>
        /// <para>Note that the value returned from the <see cref="IPermission.GetHashCode()"/> method of
        /// the permissions objects is used when comparing values in the set.
        /// </para>
        /// </remarks>
        public PermissionSet(int capacity) {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");
            permissions_ = new SecureSet<IPermission>(capacity);
        }
        #endregion

        /// <summary>
        /// Adds the specified permission to a set.
        /// </summary>
        /// <param name="permission">The permission to add to the set.</param>
        /// <returns>true if the element is added to the <see cref="PermissionSet"/> object; false if the
        /// element is already present. Note that the value returned from the GetHashCode() method of the
        /// permissions objects is used when comparing values in the set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="permission"/> is a null reference.</exception>
        public bool Add(IPermission permission) {
            if (permission == null)
                throw new ArgumentNullException("permission");
            return permissions_.Add(permission);
        }

        /// <summary>
        /// Removes the specified element from a <see cref="PermissionSet"/> object.
        /// </summary>
        /// <param name="permission">The element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method
        /// returns false if <paramref name="permission"/> is not found in the <see cref="PermissionSet"/>
        /// object. Note that the value returned from the GetHashCode() method of the permissions objects
        /// is used when comparing values in the set. So, if two permissions with the same value coexists
        /// in the set, this method, depending if the <paramref name="permission"/> object overrides the
        /// <see cref="object.Equals(object)"/> method, could remove only one of them.</returns>
        /// <remarks>
        /// If <paramref name="permission"/> is null a exception will not be throw and this method simple
        /// returns false.
        /// </remarks>
        public bool Remove(IPermission permission) {
            if (permission == null)
                return false;
            return permissions_.Remove(permission);
        }

        /// <summary>
        /// Determines whether the <see cref="PermissionSet"/> contains the specified element.
        /// </summary>
        /// <param name="permission">The principal to locate in the <see cref="PermissionSet"/>.</param>
        /// <returns>true if the item is found in the collection; otherwise, false.</returns>
        /// <remarks>
        /// This method is a O(1) operation.
        /// </remarks>
        public bool Contains(IPermission permission) {
            return permissions_.Contains(permission);
        }

        /// <summary>
        /// Gets the number of elements that are contained in the set.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="PermissionSet"/></value>
        /// <remarks>
        /// The capacity of a <see cref="PermissionSet"/> object is the number of elements that the
        /// object can hold. A <see cref="PermissionSet"/> object's capacity automatically increases
        /// as elements are added to the object.
        /// <para>
        /// The capacity is always greather than or equal to <see cref="Count"/>. If count exceeds the
        /// capacity while adding elements, the capacity is increased by automatically reallocating the
        /// internal storage before copying the old elements and adding the new elements.
        /// </para>
        /// <para>
        /// Getting the value of this property is an O(1) operation.
        /// </para>
        /// </remarks>
        public int Count {
            get { return permissions_.Count; }
        }

        /// <summary>
        /// Removes all items from the <see cref="PermissionSet"/>
        /// </summary>
        /// <remarks>
        /// <see cref="Count"/> is set to zero, and references to other objects from elements of the
        /// collection are also released. The capacity remains unchanged.
        /// <para>
        /// This method is an O(n) operation, where n is the capacity of the set.
        /// </para>
        /// </remarks>
        public void Clear() {
            permissions_.Clear();
        }

        /// <summary>
        /// Checks to see if the specified permission is implied by the collection of
        /// <see cref="IPermission"/> objects held in this set.
        /// </summary>
        /// <param name="permission">The <see cref="IPermission"/> object to compare.</param>
        /// <returns>true if the <paramref name="permission"/> is implied by the permissions in the set;
        /// otherwise, false.</returns>
        /// <remarks>
        /// This method returns true if one of the <see cref="IPermission"/> objects in the set implies
        /// the specified permission.
        /// </remarks>
        public bool Implies(IPermission permission) {
            foreach (IPermission permission_in_set in this) {
                if (permission_in_set.Implies(permission)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Compares the specified set object with this set for equality.
        /// </summary>
        /// <param name="obj">The set to be compared for equality with this instance.</param>
        /// <returns>true if the given object is also a <see cref="PermissionSet"/> and two instances are
        /// equivalent.</returns>
        /// <remarks>
        /// The hash code of a set is compute by using your elements. So if two set objects has
        /// the same collection of elements them they are equals.
        /// </remarks>
        public override bool Equals(object obj) {
            PermissionSet set = obj as PermissionSet;
            return Equals(set);
        }

        /// <summary>
        /// Compares the specified set object with this set for equality.
        /// </summary>
        /// <param name="set">The set to be compared for equality with this instance.</param>
        /// <returns>true if the given object is also a <see cref="PermissionSet"/> and two instances are
        /// equivalent.</returns>
        /// <remarks>
        /// The hash code of a set is compute by using your elements. So if two set objects has
        /// the same collection of elements them they are equals.
        /// </remarks>
        public bool Equals(PrincipalSet set) {
            return (set.Count == Count && set.GetHashCode() == GetHashCode());
        }

        /// <summary>
        /// Gets the hash code value for this set.
        /// </summary>
        /// <returns>The hash code for this set object.</returns>
        /// <remarks>
        /// The required hash code behavior for set objects is the followig:
        /// <list type="bullet">
        /// <item>Whenever it is invoked on the same set object more than once during an execution
        /// of a application, the GetHashCode methos must consistently return the same integer. This
        /// integer does not remain consistent from one execution of an application to another execution
        /// to another execution of the same application</item>
        /// <item>
        /// If two secure set objects are equal according to the equals method, then calling the
        /// GetHashCode method on each of the two principal objects must produce the same integer result.
        /// </item>
        /// </list>
        /// </remarks>
        /// <seealso cref="Object.GetHashCode()"/>
        /// <see cref="Object.Equals(System.Object)"/>
        public override int GetHashCode() {
            int i = 0;
            foreach (IPermission permission in permissions_) {
                i ^= permission.GetHashCode();
            }
            return i;
        }

        #region IEnumerable && IEnumerable<T>
        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="PermissionSet"/>.
        /// </summary>
        /// <returns>A enumerator for the set.</returns>
        public IEnumerator<IPermission> GetEnumerator() {
            return permissions_.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="PermissionSet"/>.
        /// </summary>
        /// <returns>A enumerator for the set.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return permissions_.GetEnumerator();
        }
        #endregion
    }
}