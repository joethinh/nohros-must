using System;
using System.Collections.Generic;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// A <see cref="ISubject"/> represents a grouping of related information for
  /// a single entity, such as a person or service. Such information includes
  /// the subject's permissions as well as its security-related attributes (
  /// passwords and cryptographic keys, for example).
  /// <para>
  /// Subjects may potentially have multiple permissions. Each permission
  /// represented as a <see cref="IPermission"/> object within the subject.
  /// </para>
  /// <para>
  /// Subjects may also have multiple identities. Each identity is represented
  /// as a <see cref="IPrincipal"/> within the subject. Principals simply bind
  /// names to a subject. For example, a subject that happens to be a person,
  /// Daniela, might have two Principals: one which binds "Daniela Bar", the
  /// name of her driver license, to the subject, and other which binds,
  /// "999-99-9999", the number on her student identification card, to the
  /// subject. Both principals refer to the same subject even through each has
  /// a different name.
  /// </para>
  /// <para>
  /// To retrieve all the principals associated with a subject, get the value
  /// of the <see cref="ISubject.Principals"/> property. To retrieve all the
  /// permissions associated with a subject, get the value of the
  /// <see cref="ISubject.Permissions"/> property. To modify the returned
  /// collection of permissions or principals, use methods defined in the
  /// <see cref="PermissionSet"/> and <see cref="PrincipalSet"/> classes,
  /// respectively. For example:
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
  public interface ISubject
  {
    /// <summary>
    /// Determines whether the access request indicated by the specified
    /// permission should be granted or denied for the underlying subject.
    /// </summary>
    /// <param name="permission">
    /// The requested permission.
    /// </param>
    /// <returns>
    /// <c>true</c> if the access request is permitted; otherwise <c>false</c>.
    /// </returns>
    bool CheckPermission(IPermission permission);

    /// <summary>
    /// Compares the specified subject with this subject for for equality.
    /// </summary>
    /// <param name="subject">
    /// The subject to be compared for equality with this instance.
    /// </param>
    /// <returns>
    /// <c>true</c> if the given object is also a <see cref="ISubject"/> and
    /// two instances are equivalent.
    /// </returns>
    /// <remarks>
    /// The hash code of a subject is compute by using your principals. So if
    /// two subject objects has the same collection of principals them they are
    /// equals.
    /// </remarks>
    bool Equals(ISubject subject);

    /// <summary>
    /// Gets a set of permissions associated with this subject.
    /// Each <see cref="IPermission"/> represents an access to a system
    /// resource.
    /// </summary>
    /// <remarks>
    /// The returned <see cref="PermissionSet"/> is backed by this subject's
    /// internal permisssion set. Any modification to the returned set
    /// <see cref="PermissionSet"/> object affects the internal permission set
    /// as well.
    /// </remarks>
    ISet<IPermission> Permissions { get; }

    /// <summary>
    /// Gets a set of principals associated with this subject.
    /// Each <see cref="IPrincipal"/> represents an identity for this subject.
    /// </summary>
    /// <remarks>
    /// The returned <see cref="PrincipalSet"/> is backed by this subject's
    /// internal principal set. Any modification to the returned set
    /// <see cref="PrincipalSet"/> object affects the internal principal set
    /// as well.
    /// </remarks>
    ISet<IPrincipal> Principals { get; }
  }
}
