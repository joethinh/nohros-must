using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Provides a generic implementation of the <see cref="IPrincipal"/> interface.
    /// </summary>
    public class Principal : IPrincipal
    {
        /// <summary>
        /// The principal identification.
        /// </summary>
        protected string name_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="Principal"/> class by using specified principal
        /// name.
        /// </summary>
        /// <param name="name">The principal identification.</param>
        public Principal(string name) {
            name_ = name;
        }
        #endregion

        /// <summary>
        /// Determines whether this instance of <see cref="IPrincipal"/> and a specified object, which
        /// must also be a <see cref="IPrincipal"/> object, refers to the same princiapal.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns>true if <paramref name="obj"/> is a <see cref="IPrincipal"/> and its value is
        /// the same as this instance; otherwise, false.</returns>
        /// <remarks>
        /// This methods should not throw any exception, even if the specified object is null.
        /// </remarks>
        public override bool Equals(object obj) {
            IPrincipal principal = obj as IPrincipal;
            return Equals(principal);
        }

        /// <summary>
        /// Determines whether this instance of <see cref="IPrincipal"/> and another specified
        /// <see cref="IPrincipal"/> refers to the same principal.
        /// </summary>
        /// <param name="principal">A <see cref="IPrincipal"/> object.</param>
        /// <returns>true if the value of the <paramref name="perm"/> parameter is the same as this
        /// instance; otherwise, false.</returns>
        /// <remarks>
        /// This methods should not throw any exception, even if the specified principal is null.
        /// </remarks>
        public bool Equals(IPrincipal principal) {
            if (principal == null)
                return false;
            return string.Compare(principal.Name, name_) == 0;
        }

        /// <summary>
        /// Determines whether this instance of <see cref="IPrincipal"/> and another specified
        /// <see cref="IPrincipal"/> refers to the same principal.
        /// </summary>
        /// <param name="principal">A <see cref="IPrincipal"/> object.</param>
        /// <returns>true if the value of the <paramref name="perm"/> parameter is the same as this
        /// instance; otherwise, false.</returns>
        /// <remarks>
        /// This methods should not throw any exception, even if the specified principal is null.
        /// </remarks>
        public bool Equals(Principal principal) {
            if (principal == null)
                return false;
            return string.Compare(principal.name_, name_) == 0;
        }

        /// <summary>
        /// Gets the hash code value for this principal object.
        /// </summary>
        /// <returns>The hash code for this principal object.</returns>
        /// <remarks>
        /// The required hash code behavior for principal objects is the followig:
        /// <list type="bullet">
        /// <item>Whenever it is invoked on the same principal object more than once during an execution
        /// of a application, the GetHashCode methos must consistently return the same integer. This
        /// integer does not remain consistent from one execution of an application to another execution
        /// to another execution of the same application</item>
        /// <item>
        /// If two principal objects are equal according to the equals method, then calling the
        /// GetHashCode method on each of the two principal objects must produce the same integer result.
        /// </item>
        /// </list>
        /// </remarks>
        /// <seealso cref="Object.GetHashCode()"/>
        /// <see cref="Object.Equals(System.Object)"/>
        public override int GetHashCode() {
            return name_.GetHashCode();
        }

        /// <summary>
        /// Gets the name of the principal.
        /// </summary>
        public string Name {
            get { return name_; }
        }
    }
}
