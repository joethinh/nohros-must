using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

using Nohros.Data;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// A <see cref="Subject"/> represents a grouping of related information for a single entity, such as
    /// a person. Such information includes the subject's permissions as well as its security-related
    /// attributes(passwords and cryptographic keys, for example).
    /// <para>
    /// Subjects may potentially have multiple permissions. Each permission represented as a
    /// <see cref="IPermission"/> object within the subject.
    /// </para>
    /// <para>
    /// A subject may also own security-related attributes, which are referred to as credentials.
    /// Sensitive credentials that require special protection, such as private cryptographic keys, are
    /// stored within a private credential list. Credentials intended to be shared, such as public key
    /// certificates or Kerberos server tickets are stored within a public credential.
    /// </para>
    /// <para>
    /// To retrieve all the permissions associated with a subject, get the value of the
    /// <see cref="Subject.Permissions"/> property. To retrieve all the public or private credentials
    /// beloging to a subject, get the value of the <see cref="Subject.PublicCredentials"/> property or
    /// <see cref="Subject.PrivateCredentials"/> property, respectively. To modify the returned
    /// collection of permissions and credentials, use methods defined in the <see cref="IPermissionSet"/>
    /// class. For example:
    /// <example>
    ///     <code>
    ///         Subject subject;
    ///         IPermission permission;
    ///         Object credential;
    ///         
    ///         // add a permission and credential to the subject.
    ///         subject.Permissions.Add(permission);
    ///         subject.PublicCredentials().Add(credetial);
    ///     </code>
    /// </example>
    /// </para>
    /// </summary>
    public partial class Subject
    {
#if NET20
        IList<IPermission> permissions_;
#else
        HashSet<IPermission> permissions_;
#endif
        string _id;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the Subject class with an empty set of permissions and
        /// credentials.
        /// </summary>
        public Subject()
        {
            permissions_ = new List<IPermission>();
        }

        /// <summary>
        /// Create an instance of a Subject with Permissions and SubjectLoader delegate.
        /// </summary>
        /// <param name="permissions">The subject's permissions arrray.</param>
        /// <exception cref="ArgumentNullException">permissions is null.</exception>
        public Subject(IPermission[] permissions) {
            if (permissions == null)
                throw new ArgumentNullException("permissions");

            for (int i = 0, j = permissions.Length; i < j; i++) {
                permissions_.Add(permissions[i]);
            }
        }
        #endregion

        /// <summary>
        /// Determines whether the access request indicated by the specified permission should be allowed
        /// or denied, based on the current security context.
        /// </summary>
        /// <param name="permission">The requested permission</param>
        /// <returns>true if the access request is permitted; otherwise false</returns>
        public bool CheckPermission(IPermission permission)
        {
            for(int i=0, j=permissions_.Count;i<j;i++) {
                if (permissions_[i].Implies(permission))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the <see cref="Dictionary&lt;TKey, TValue&gt;"/> of <see cref="IPermission"/> associated
        /// with this Subject. Each <see cref="IPermission"/> represents an access to a system resource.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="Dictionary&lt;TKey, TValue&gt;"/> is backed by this Subject's internal
        /// <see cref="IPermission"/> <see cref="Dictionary&lt;TKey, TValue&gt;"/>. Any modification to the
        /// returned <see cref="Dictionary&lt;TKey, TValue&gt;"/> affects the internal <see cref="IPermission"/>
        /// <see cref="Dictionary&alt;TKey, TValue&gt;"/> as well.
        /// </remarks>
        public IList<IPermission> Permissions
        {
            get { return permissions_; }
        }

        /// <summary>
        /// Gets or sets a value that uniquely identifies the subject within the application.
        /// </summary>
        public string ID
        {
            get { return _id; }
            set
            {
                if (value == null)
                    Thrower.ThrowArgumentNullException(ExceptionArgument.value);

                // the id value is set after a sucessfull login attempt.
                // at this point we have all needed information to cache the subject.
                if(_id != value)
                {
                    _id = value;
                    Subject.Add(this);
                }
            }
        }
    }
}