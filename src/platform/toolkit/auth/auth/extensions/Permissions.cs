using System;
using System.Collections.Generic;
using System.Linq;

namespace Nohros.Security.Auth.Extensions
{
  public static class Permissions
  {
    /// <summary>
    /// Checks to see if the given <paramref name="permission"/> is implied by
    /// the given collection of <paramref name="permissions"/>
    /// </summary>
    /// <param name="permission">
    /// A <see cref="IPermission"/> object to check for.
    /// </param>
    /// <param name="permissions">
    /// A <see cref="ISet{T}"/> of <see cref="IPermission"/> to check for.
    /// </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="permission"/> is
    /// implied by the permissions in the <paramref name="permissions"/> set;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method returns <c>true</c> if one of the <see cref="IPermission"/>
    /// object in the given <paramref name="permissions"/> set implies the
    /// specified permission.
    /// </remarks>
    /// <see cref="IPermission.Implies"/>
    public static bool Implies(this ISet<IPermission> permissions,
      IPermission permission) {
      return
        permissions.Any(
          permission_in_set => permission_in_set.Implies(permission));
    }
  }
}
