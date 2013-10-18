using System;
using System.Collections.Generic;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// A implementation of the <see cref="IAuthenticationInfo"/> to support the
  /// most widely-used authentication mechanism.
  /// </summary>
  public class AuthenticationInfo : IAuthenticationInfo
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationInfo"/>
    /// class by using a value taht indicates if the authentication process
    /// has failed or not.
    /// </summary>
    /// <param name="authenticated">
    /// A value that indicates the sucess of the authentication process.
    /// <c>true</c> is the authentication process completed successfully;
    /// otherwise, <c>false</c>.
    /// </param>
    public AuthenticationInfo(bool authenticated) {
      Authenticated = authenticated;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationInfo"/>
    /// class by using a value taht indicates if the authentication process
    /// has failed or not.
    /// </summary>
    /// <param name="authenticated">
    /// A value that indicates the sucess of the authentication process.
    /// <c>true</c> is the authentication process completed successfully;
    /// otherwise, <c>false</c>.
    /// </param>
    /// <param name="principals">
    /// A <see cref="ISet{T}"/> of principals associated with the
    /// authentication process.
    /// </param>
    /// <param name="permissions">
    /// A <see cref="ISet{T}"/> of permissions associated with the
    /// authentication process.
    /// </param>
    public AuthenticationInfo(bool authenticated, ISet<IPrincipal> principals,
      ISet<IPermission> permissions) {
      Authenticated = authenticated;
      Principals = principals;
      Permissions = permissions;
    }
    #endregion

    /// <inheritdoc/>
    public bool Authenticated { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="ISubject"/> that is assocaited with the
    /// authentication process.
    /// </summary>
    public ISubject Subject { get; set; }

    /// <summary>
    /// A <see cref="ISet{T}"/> of <see cref="IPrincipal"/> objects that is
    /// associated with the authentication process.
    /// </summary>
    public ISet<IPrincipal> Principals { get; private set; }

    /// <summary>
    /// A <see cref="ISet{T}"/> of <see cref="IPermission"/> objects that is
    /// associated with the authentication process.
    /// </summary>
    public ISet<IPermission> Permissions { get; private set; }
  }
}
