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
    /// a person or service. Such information includes the subject's permissions as well as its
    /// security-related attributes(passwords and cryptographic keys, for example).
    /// <para>
    /// Subjects may potentially have multiple permissions. Each permission represented as a
    /// <see cref="IPermission"/> object within the subject.
    /// </para>
    /// <para>
    /// Subjects may also have multiple identities. Each identity is represented as a
    /// <see cref="IPrincipal"/> within the subject. Principals simply bind names to a subject. For
    /// example, a subject that happens to be a person, Daniela, might have two Principals: one which
    /// binds "Daniela Bar", the name of her driver license, to the subject, and other which binds,
    /// "999-99-9999", the number on her student identification card, to the subject. Both principals
    /// refer to the same subject even through each has a different name.
    /// </para>
    /// <para>
    /// To retrieve all the principals associated with a subject, get the value of the
    /// <see cref="Subject.Principals"/> property. To retrieve all the permissions associated with a
    /// subject, get the value of the <see cref="Subject.Permissions"/> property. To modify the returned
    /// collection of permissions or principals, use methods defined in the
    /// <see cref="PermissionSet"/> and <see cref="PrincipalSet"/> classes, respectively. For example:
    /// <example>
    ///     <code>
    ///         Subject subject;
    ///         IPermission permission;
    ///         IPrincipal principal;
    ///         
    ///         // add a principal and permission to the subject.
    ///         subject.Permissions.Add(permission);
    ///         subject.Principals.Add(principal);
    ///     </code>
    /// </example>
    /// </para>
    /// </summary>
    public partial class Subject
    {
        PermissionSet permissions_;
        PrincipalSet principals_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the Subject class with an empty set of permissions and
        /// principals.
        /// </summary>
        public Subject() {
            permissions_ = new PermissionSet();
            principals_ = new PrincipalSet();
        }

        /// <summary>
        /// Creates an instance of a <see cref="Subject"/> class with the specified permissions and a
        /// empty set of princiapals.
        /// </summary>
        /// <param name="permissions">The subject's permissions collection.</param>
        /// <exception cref="ArgumentNullException">permissions is null.</exception>
        public Subject(PermissionSet permissions) {
            if (permissions == null)
                throw new ArgumentNullException("permissions");

            permissions_ = permissions;
            principals_ = new PrincipalSet();
        }

        /// <summary>
        /// Creates an instance of a <see cref="Subject"/> class with the specified principals and a
        /// empty set of permissions.
        /// </summary>
        /// <param name="principals">The subject's principals collection.</param>
        /// <exception cref="ArgumentNullException">principals is null.</exception>
        public Subject(PrincipalSet principals) {
            if (principals == null)
                throw new ArgumentNullException("principals");

            permissions_ = new PermissionSet();
            principals_ = principals;
        }

        /// <summary>
        /// Creates an instance of a <see cref="Subject"/> class with the specified principals set and
        /// permissions set.
        /// </summary>
        /// <param name="permissions">The subject's principals collection.</param>
        /// <param name="principals">The subject's principals collection.</param>
        /// <exception cref="ArgumentNullException">principals is null or permissions is null.</exception>
        public Subject(PermissionSet permissions, PrincipalSet principals) {
            if (permissions == null || principals == null)
                throw new ArgumentNullException((permissions == null) ? "permissions" : "principals");

            permissions_ = permissions;
            principals_ = principals;
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
        /// Gets a set of permissions associated with this subject. Each <see cref="IPermission"/>
        /// represents an access to a system resource.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="PemissionSet"/> is backed by this subject's internal permisssion set.
        /// Any modification to the returned set <see cref="PermissionSet"/> object affects the internal
        /// permission set as well.
        /// </remarks>
        public PermissionSet Permissions {
            get { return permissions_; }
        }

        /// <summary>
        /// Gets a set of principals associated with this subject. Each <see cref="IPrincipal"/>
        /// represents an identity for this subject.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="PrincipalSet"/> is backed by this subject's internal principal set.
        /// Any modification to the returned set <see cref="PrincipalSet"/> object affects the internal
        /// principal set as well.
        /// </remarks>
        public PrincipalSet Principals {
            get { return principals_; }
        }

        /// <summary>
        /// Compares the specified object with this subject for for equality.
        /// </summary>
        /// <param name="obj">Object to be compared for equality with this subject.</param>
        /// <returns>true if the given object is also a <see cref="Subject"/> and two instances are
        /// equivalent.</returns>
        /// <remarks>
        /// Two <see cref="Subject"/> instances are equal if their <see cref="IPermission"/> set are
        /// equal.
        /// </remarks>
        public override bool Equals(object obj) {
            Subject subject = obj as Subject;
            if (obj == null)
                return false;
            return Equals(subject);
        }

        /// <summary>
        /// Compares the specified subject with this subject for for equality.
        /// </summary>
        /// <param name="subject">The subject to be compared for equality with this instance.</param>
        /// <returns>true if the given obhecr is also a <see cref="Subject"/> and two instabces are
        /// equivalent.</returns>
        /// <remarks>
        /// Two <see cref="Subject"/> instances are equal if their <see cref="IPermission"/> set are
        /// equal.
        /// </remarks>
        public bool Equals(Subject subject) {
            return (subject.permissions_ == permissions_);
        }

        /// <summary>
        /// Gets the hash code for this subject.
        /// </summary>
        /// <returns>A hashcode for this subject.</returns>
        public override int GetHashCode() {
            int i=0;
            foreach(IPermission permission in permissions_) {
                i ^= permission.GetHashCode();
            }
            return i;
        }
    }
}