using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Represent a set of permissions.
    /// </summary>
    public class PermissionSet
    {
        Dictionary<int, IPermission> permissions_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionSet"/> class that is empty.
        /// </summary>
        public PermissionSet() {
            permissions_ = new Dictionary<int,IPermission>();
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
            if (permissions == null)
                throw new ArgumentNullException("permissions");

            foreach(IPermission permission_to_add in permissions) {
                if(permission_to_add != null) {
                    permissions_[permission_to_add.GetHashCode()] = permission_to_add;
                }
            }
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

            int hash_code = permission.GetHashCode();
            if (permissions_.ContainsKey(hash_code)) {
                permissions_[hash_code] = permission;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Removes the specified element from a <see cref="PermissionSet"/> object.
        /// </summary>
        /// <param name="permission">The element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method
        /// returns false if <paramref name="permission"/> is not found in the <see cref="PermissionSet"/>
        /// object. Note that the default equality comparer is used when comparing values in the set.
        /// So, if two permissions with the same value coexists in the set, this method, depending if
        /// the <paramref name="permission"/> object overrides the <see cref="Object.Equals"/> method,
        /// could remove only one of them.</returns>
        public bool Remove(IPermission permission) {
            if (permission == null)
                throw new ArgumentNullException("permission");
            return permissions_.Remove(permission.GetHashCode());
        }

        public void Implies() {
        }
    }
}