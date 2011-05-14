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

#if NET35 || NET40
        HashSet<IPermission> permissions_;
#else
        List<IPermission> permissions_;
#endif

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionSet"/> class that is empty.
        /// </summary>
        public PermissionSet() {
#if NET35 || NET40
            permissions_ = new HashSet<IPermission>();
#else
            permissions_ = new List<IPermission>();
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionSet"/> class that contains elements
        /// copied from the specified collection, and has sufficient capacity to accomodate the number of
        /// elements copied.
        /// </summary>
        /// <param name="permissions">The collection whose elements are copied to the new set.</param>
        /// <exception cref="ArugmentNullException"><paramref name="permissions"/> is a null reference.</exception>
        /// <remarks>
        /// The capacity of a <see cref="PermissionSet"/> object is the number of elements that object
        /// can hold. A <see cref="PermissionSet"/> object's capacity automatically increases as elements
        /// are added to the object.
        /// <para>
        /// If the specified collection of permissions contains duplicates, the set will contain one of
        /// each unique element. No exception will be throw. Therefore, the size of the resulting set is
        /// not identical to the size of <paramref name="permissions"/>. Note that the default equality
        /// comparer is used when comparing values in the set. So, permissions with the same value could
        /// be coexists in the set since they are distinct instances.
        /// </para>
        /// <para>
        /// This constructor is an O(n^2) operation for .NET 2.0 and an O(n) operation for .NET 3.5 and
        /// later versions.
        /// </para>
        /// </remarks>
        public PermissionSet(IEnumerable<IPermission> permissions) {
            if (permissions == null)
                throw new ArgumentNullException("permissions");

            foreach (IPermission permission in permissions) {
#if NET35 || NET40
                permissions_.Add(permission);
#else
                IPermission permission_to_add = null;
                for (int i = 0, j = permissions_.Count; i < j; i++) {
                    permission_to_add = permissions_[i];
                    if (permission_to_add == permission) {
                        permission_to_add = null;
                        break;
                    }
                }
                if (permission_to_add != null)
                    permissions_.Add(permission);
#endif
            }
        }
        #endregion

        /// <summary>
        /// Adds the specified permission to a set.
        /// </summary>
        /// <param name="permission">The permission to add to the set.</param>
        /// <returns>true if the element is added to the <see cref="PermissionSet"/> object; false if the
        /// element is already present. Note that the default equality comparer is used when comparing
        /// values in the set. So, permissions with the same value could be coexists in the set since
        /// they are distinct instances.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="permission"/> is null.</exception>
        public bool Add(IPermission permission) {
#if NET35 || NET40
                permissions_.Add(permission);
#else
            IPermission permission_to_add = null;
            for (int i = 0, j = permissions_.Count; i < j; i++) {
                permission_to_add = permissions_[i];
                if (permission_to_add == permission) {
                    permission_to_add = null;
                    break;
                }
            }
            if (permission_to_add != null)
                permissions_.Add(permission);
#endif
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
        }

        /// <summary>
        /// Removes all permission whose name is equals to <paramref name="permission_name"/>.
        /// </summary>
        /// <param name="permission_name">The name of the permission to remove.</param>
        /// <returns>true if the at least one element is successfully found and removed; otherwise, false.
        /// This method returns false if permissions whose name is <paramref name="permission_name"/> are
        /// not found.</returns>
        /// <remarks>
        /// This method performs a lnear search; therefore, this method is an O(n) operation, where n is
        /// <see cref="PermissionSet.Count"/>.
        /// </remarks>
        public bool Remove(string permission_name) {
            int[] permissions_to_remove = new IPermission[permissions_.Count];
            IPermission permission;
            for(int i=0,j=permissions_.Count;i<j;i++) {
                permission = permissions_[i];
                if (string.Compare(permission_name, permission.Name) == 0) {
                }
            }
        }
    }
}