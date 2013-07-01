using System;
using System.Runtime.Serialization;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Provides the abstract base class for the <see cref="IPermission"/> interface.
    /// <para>
    /// This class implements the <see cref="IPermission.Name"/>, <see cref="IPermission.Mask"/> and the
    /// <see cref="Object.GetHashCode"/> method.
    /// </para>
    /// </summary>
    public abstract class PermissionBase : IPermission
    {
        /// <summary>
        /// The name of the permission name.
        /// </summary>
        protected string name_;

        /// <summary>
        /// The permission actions bitmask.
        /// </summary>
        protected long mask_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionBase"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the permission.</param>
        /// <remarks>
        /// Permissions objects created by using this constructor have the value of the actions
        /// bitmask equals to zero.
        /// </remarks>
        protected PermissionBase(string name) {
            name_ = name;
            mask_ = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionBase"/> class by using the specified
        /// permission name and action bit mask.
        /// </summary>
        protected PermissionBase(string name, long mask) {
            name_ = name;
            mask_ = mask;
        }
        #endregion

        /// <summary>
        /// Checks if the specified permission's actions are "implied by" this object's actions. This must
        /// be implemented by derived classes of IPermission, as they are the only ones that can impose
        /// semantics on a IPermission object. The implies method is used by the SecurityContext to determine
        /// whether or not a requested permission is implied by another permission that is known to be valid
        /// in the current execution context.
        /// </summary>
        /// <param name="perm">The permission to check against.</param>
        /// <returns>true if the specified permission is implied by this object, false if not.</returns>
        public abstract bool Implies(IPermission perm);

        /// <summary>
        /// Serves as a hash function for the <see cref="PermisionBase"/> class.
        /// </summary>
        /// <returns>A hash code for the current permission.</returns>
        /// <remarks>
        /// The hash code is calculated by invoking the GethashCode method of the resulting string of the
        /// concatenation between the permission name and the string representation of the actions bitmask
        /// flag. In this way we can guarantee that two permissions with the same name and actions bitmask
        /// have the same hash code.
        /// </remarks>
        public override int GetHashCode() {
            return string.Concat(name_, mask_).GetHashCode();
        }

        /// <summary>
        /// Determines whether this instance of <see cref="IPermission"/> and a specified object, which
        /// must also be a <see cref="IPermission"/> object, refers to the same permission.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns>true if the specified object is an <see cref="IPermission"/> and its name and action
        /// mask value are the same as this instance; otherwise, false.</returns>
        /// <remarks>
        /// <para>
        /// This class uses the permission name and its action mask when comparing permissions. This
        /// method performs an ordinal(case-insensitive and culture-insensitive) comparison.
        /// </para>
        /// This methods does not throw any exception, even if the specified permission is null.
        /// </remarks>
        public override bool Equals(object obj) {
            IPermission permission = obj as IPermission;
            return Equals(permission);
        }

        /// <summary>
        /// Determines whether this instance of <see cref="IPermission"/> and another specified
        /// <see cref="IPermission"/> refers to the same permission.
        /// </summary>
        /// <param name="perm">A <see cref="IPermission"/> object.</param>
        /// <returns>true if the name and action mask value of the <paramref name="perm"/> parameter is
        /// the same as this instance; otherwise, false.</returns>
        /// <remarks>
        /// <para>
        /// This class uses the permission name and its action mask when comparing permissions. This
        /// method performs an ordinal(case-insensitive and culture-insensitive) comparison.
        /// </para>
        /// This methods does not throw any exception, even if the specified permission is null.
        /// </remarks>
        public bool Equals(IPermission perm) {
            if (perm == null)
                return false;
            return (string.Compare(perm.Name, this.name_) == 0 && perm.Mask == mask_);
        }

        /// <summary>
        /// Gets the name of this permission.
        /// </summary>
        public string Name {
            get { return name_; }
        }

        /// <summary>
        /// Gets the actions bitmask that tells the actions that are permited for the object.
        /// </summary>
        public long Mask {
            get { return mask_; }
        }
    }
}