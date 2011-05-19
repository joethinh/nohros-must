using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Represents an abstract notion of a principal, which can be used to represent any entity such as
    /// an individual, a corporation, and a login id.
    /// </summary>
    public interface IPrincipal
    {
        /// <summary>
        /// Gets the name of the principal.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Determines whether this instance of <see cref="IPrincipal"/> and a specified object, which
        /// must also be a <see cref="IPrincipal"/> object, refers to the same permission.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns>true if <paramref name="obj"/> is a <see cref="IPrincipal"/> and its value is
        /// the same as this instance; otherwise, false.</returns>
        /// <remarks>
        /// This methods should not throw any exception, even if the specified object is null.
        /// </remarks>
        bool Equals(object obj);

        /// <summary>
        /// Determines whether this instance of <see cref="IPrincipal"/> and another specified
        /// <see cref="IPrincipal"/> refers to the same permission.
        /// </summary>
        /// <param name="principal">A <see cref="IPrincipal"/> object.</param>
        /// <returns>true if the value of the <paramref name="perm"/> parameter is the same as this
        /// instance; otherwise, false.</returns>
        /// <remarks>
        /// This methods should not throw any exception, even if the specified permission is null.
        /// </remarks>
        bool Equals(IPrincipal principal);

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
