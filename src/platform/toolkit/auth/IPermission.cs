using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Represents access to a resource. All permissions have a name(whose interpretation depends on the
    /// subclass), as well functions for defining the semantics of the particular permission class.
    /// </summary>
    /// <para>
    /// Most permission objects also include an "actions" bitmask that tells the action that are
    /// permitted for the object. For example, for a FilePermission object, the permission name could be
    /// the pathname of a file(or directory), and the action list(ex. "read:0x1", "write:0x2") specifies
    /// which actions are granted for the specified file(or files in the specified directory).
    /// The actions list is optional for permission objects that do not need such a list; you either have
    /// a named permission(such as "system.exit") or you do not.
    /// <para>
    /// <see cref="IPermission"/> object have two important method that must be implemented by each class.
    /// They are <see cref="IPermission.Implies(IPermission)"/> and <see cref="Object.GetHashCode()"/>.
    /// </para>
    /// <para>
    /// The <see cref="IPermission.Implies(IPermission)"/> method is used to compare permissions. Basically,
    /// "permission p1 implies permission p2" means that if one is granted permission p1, one is
    /// naturally granted permission p2. Thus, this is not an equality test, but rather more a subset
    /// test.
    /// </para>
    /// <para>
    /// The msdn doc - http://msdn.microsoft.com/en-us/library/system.object.gethashcode.aspx) said:
    ///     "The GetHashCode() method is suitable for use in data structures such as a hash table. It
    ///      provides a numeric value that is used to identify an object equality testing. It can also
    ///      serve as an index for an object in a collection... The default implementation of the
    ///      GetHashCode() method does not guarantee unique return values for different objects.
    ///      Furthermore, the .NET Framework. Consequently, the default implementation of this method
    ///      must not be used as a unique object identifier for hashing purposes."
    /// </para>
    /// <para>
    /// The value returned from the <see cref="Object.GetHashCode()"/> method will be used to identify
    /// equality between permission objects and classes that implements this interface should provide
    /// a way to guarantee that the value returned from the GetHashCode method is unique for different
    /// permissions. The msdn also said that the implementation of the GetHashCode method provided by
    /// the <see cref="String"/> class always returns identical hash codes for identical permission
    /// values. A good way to provide uniqueness is to use at least one of the instance fields as input.
    /// Implementers of the <see cref="IPermission"/> interface could use a combination  of the full name
    /// of the class and the string representation of the mask property as a hash input. For example,
    /// a permission whose name is "MyCompany.MyLibrary.MyPermission" could use the code above to your
    /// GetHashCode implementation:
    /// <code>
    ///     ...
    ///     long mask_;
    ///     ...
    ///     public int GetHashCode() {
    ///         return string.Concat("MyCompany.MyLibrary.MyPermission", mask_.ToString()).GetHashCode();
    ///     }
    /// </code>
    /// </para>
    /// <para>
    /// Accordingly to the msdn documentation, classes that overrides GetHashCode must also override
    /// Equals to guarantee that two objects considered equal have the same hash code. Otherwise, the
    /// classes taht rely on the behavior of the GetHashCode method might not work corretcly.
    /// </para>
    /// <para>
    /// Permission objects are similar to <see cref="String"/> objects in that they are immutable once
    /// they have been created. Classes that implements this interface should not provide methods that
    /// can change the state of a permission once it has been created.
    /// </para>
    /// </para>
    public interface IPermission
    {
        /// <summary>
        /// Checks if the specified permission's actions are "implied by" this object's actions.
        /// This must be implemented by derived classes of IPermission, as they are the only ones that
        /// can impose semantics on a IPermission object. The implies method is used by to determine
        /// whether or not a requested permission is implied by another permission that is known to be
        /// valid in the current execution context.
        /// </summary>
        /// <param name="perm">The permission to check against.</param>
        /// <returns>true if the specified permission is implied by this object, false if not.</returns>
        bool Implies(IPermission perm);

        /// <summary>
        /// Gets the name of this permission.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the actions bitmask that tells the actions that are permited for the object.
        /// </summary>
        long Mask { get; }

        /// <summary>
        /// Determines whether this instance of <see cref="IPermission"/> and a specified object, which
        /// must also be a <see cref="IPermission"/> object, refers to the same permission.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns>true if <paramref name="obj"/> is a <see cref="IPermission"/> and its value is
        /// the same as this instance; otherwise, false.</returns>
        /// <remarks>
        /// This methods should not throw any exception, even if the specified object is null.
        /// </remarks>
        bool Equals(object obj);

        /// <summary>
        /// Determines whether this instance of <see cref="IPermission"/> and another specified
        /// <see cref="IPermission"/> refers to the same permission.
        /// </summary>
        /// <param name="perm">A <see cref="IPermission"/> object.</param>
        /// <returns>true if the value of the <paramref name="perm"/> parameter is the same as this
        /// instance; otherwise, false.</returns>
        /// <remarks>
        /// This methods should not throw any exception, even if the specified permission is null.
        /// </remarks>
        bool Equals(IPermission perm);

        /// <summary>
        /// Gets the hash code value for this permission object.
        /// </summary>
        /// <returns>The hash code for this permission object.</returns>
        /// <remarks>
        /// The required hash code behavior for permission objects is the followig:
        /// <list type="bullet">
        /// <item>Whenever it is invoked on the same permission object more than once during an execution
        /// of a application, the GetHashCode methos must consistently return the same integer. This
        /// integer does not remain consistent from one execution of an application to another execution
        /// to another execution of the same application</item>
        /// <item>
        /// If two permission objects are equal according to the equals method, then calling the
        /// GetHashCode method on each of the two permission obhects must produce the same integer result.
        /// </item>
        /// </list>
        /// </remarks>
        /// <seealso cref="Object.GetHashCode()"/>
        /// <see cref="Object.Equals(System.Object)"/>
        int GetHashCode();
    }
}
