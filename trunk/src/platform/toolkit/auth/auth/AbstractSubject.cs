using System;
using System.Collections.Generic;
using Nohros.Collections;
using Nohros.Security.Auth.Extensions;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// An abstract implementation of the <see cref="ISubject"/> class.
  /// </summary>
  public abstract class AbstractSubject : ISubject
  {
    readonly EventedSet<IPermission> permissions_;
    readonly EventedSet<IPrincipal> principals_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractSubject"/> class
    /// with an empty set of permissions and principals.
    /// </summary>
    protected AbstractSubject() {
      permissions_ = new EventedSet<IPermission>(new HashSet<IPermission>());
      principals_ = new EventedSet<IPrincipal>(new HashSet<IPrincipal>());

      permissions_.ItemAdded += OnPermissionAdded;
      principals_.ItemAdded += OnPrincipalAdded;
    }
    #endregion

    /// <inheritdoc/>
    public virtual bool CheckPermission(IPermission permission) {
      return (permissions_.Implies(permission));
    }

    /// <inheritdoc/>
    public virtual ISet<IPermission> Permissions {
      get { return permissions_; }
    }

    /// <inheritdoc/>
    public virtual ISet<IPrincipal> Principals {
      get { return principals_; }
    }

    /// <inheritdoc/>
    public bool Equals(ISubject subject) {
      var s = subject as AbstractSubject;
      if ((object) s == null) {
        return false;
      }

      if (!principals_.Equals(s.Principals)) {
        return false;
      }

      return permissions_.Equals(s.Permissions);
    }

    /// <summary>
    /// Compares the specified object with this subject for for equality.
    /// </summary>
    /// <param name="obj">
    /// The object to be compared for equality with this subject.
    /// </param>
    /// <returns>
    /// <c>true</c> if the given object is also a <see cref="AbstractSubject"/>
    /// and two instances are equivalent.
    /// </returns>
    /// <remarks>
    /// The hash code of a subject is compute by using your principals. So if
    /// two subject objects has the same collection of principals them
    /// they are equals.
    /// </remarks>
    public override bool Equals(object obj) {
      var subject = obj as AbstractSubject;
      return Equals(subject);
    }

    /// <summary>
    /// Gets the hash code for this subject.
    /// </summary>
    /// <returns>A hashcode for this subject.</returns>
    public override int GetHashCode() {
      // Overflow is fine, just wrap
      unchecked {
        int hash = 17;
        foreach (IPermission permission in permissions_) {
          hash = hash*23 + permission.GetHashCode();
        }
        foreach (IPrincipal principal in principals_) {
          hash = hash*23 + principal.GetHashCode();
        }
        return hash;
      }
    }

    /// <summary>
    /// Occurs when an permission is added to the subject.
    /// </summary>
    public event PermissionEventHandler PermissionAdded;

    /// <summary>
    /// Raises the <see cref="PermissionAdded"/> event.
    /// </summary>
    /// <param name="permission"></param>
    protected virtual void OnPermissionAdded(IPermission permission) {
      Listeners.SafeInvoke<PermissionEventHandler>(PermissionAdded,
        handler => handler(permission));
    }

    /// <summary>
    /// Occurs when an <see cref="IPrincipal"/> is added to the subject.
    /// </summary>
    public event PrincipalEventHandler PrincipalAdded;

    /// <summary>
    /// Raises the <see cref="PrincipalAdded"/> event.
    /// </summary>
    /// <param name="principal"></param>
    protected virtual void OnPrincipalAdded(IPrincipal principal) {
      Listeners.SafeInvoke<PrincipalEventHandler>(PrincipalAdded,
        handler => handler(principal));
    }
  }
}
