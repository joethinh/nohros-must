using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Represents access to a resource. All permissions have a name(whose interpretation depends on the
    /// subclass), as well functions for defining the semantics of the particular IPermission class.
    /// </summary>
    /// <para>
    /// Most permission objects also include an "actions" bitmask that tells the action that are permitted
    /// for the object. For example, for a FilePermission object, the permission name could be the pathname
    /// of a file(or directory), and the action list(ex. "read:0x1", "write:0x2") specifies which actions are
    /// granted for the specified file(or files in the specified directory). The actions list is optional
    /// for Permission objects that do not need such a list; you either have a name permission ou you do not.
    /// <para>
    /// An important method that must be implemented by each derived class is the <see cref="IPermission.Implies"/>
    /// method to compare IPermissions. Basically, "permission p1 implies permission p2" means that if one is granted
    /// permission p1, one is naturally granted permission p2. Thus, this is not an equality test, but rather
    /// more a subset test.
    /// </para>
    /// <para>
    /// Permission objects are similar to <see cref="String"/> objects in that they are immutable once they
    /// have been created. Classes that implements this interface should not provide methods that can change
    /// the state of a permission once it has been created.
    /// </para>
    /// </para>
    public interface IPermission: ISerializable
    {
        /// <summary>
        /// Checks if the specified permission's actions are "implied by" this object's actions. This must
        /// be implemented by derived classes of IPermission, as they are the only ones that can impose
        /// semantics on a IPermission object. The implies method is used by the SecurityContext to determine
        /// whether or not a requested permission is implied by another permission that is known to be valid
        /// in the current execution context.
        /// </summary>
        /// <param name="perm">The permission to check against</param>
        /// <returns>true if the specified permission is implied by this object, false if not</returns>
        bool Implies(IPermission perm);

        /// <summary>
        /// Gets the name of this permission.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the actions bitmask that tells the actions that are permited for the object.
        /// </summary>
        long Mask { get; }
    }
}
