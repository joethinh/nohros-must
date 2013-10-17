using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// An abstract implementation of the <see cref="ISubject"/> class.
  /// </summary>
  public abstract class AbstractSubject : ISubject
  {
    readonly PermissionSet permissions_;
    readonly PrincipalSet principals_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractSubject"/> class with an
    /// empty set of permissions and principals.
    /// </summary>
    protected AbstractSubject() {
      permissions_ = new PermissionSet();
      principals_ = new PrincipalSet();
    }
    #endregion

    /// <inheritdoc/>
    public virtual bool CheckPermission(IPermission permission) {
      return (permissions_.Implies(permission));
    }

    /// <inheritdoc/>
    public virtual PermissionSet Permissions {
      get { return permissions_; }
    }

    /// <inheritdoc/>
    public virtual PrincipalSet Principals {
      get { return principals_; }
    }

    /// <summary>
    /// Compares the specified object with this subject for for equality.
    /// </summary>
    /// <param name="obj">
    /// The object to be compared for equality with this subject.
    /// </param>
    /// <returns>
    /// <c>true</c> if the given object is also a <see cref="AbstractSubject"/> and
    /// two instances are equivalent.
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
    /// Gets the hash code for this subject.
    /// </summary>
    /// <returns>A hashcode for this subject.</returns>
    public override int GetHashCode() {
      int i = 0;
      foreach (IPermission permission in permissions_) {
        i ^= permission.GetHashCode();
      }
      foreach (IPrincipal principal in principals_) {
        i ^= principal.GetHashCode();
      }
      return i;
    }
  }
}
