using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// The BasicPermission class  is the most basic implmentation of the
  /// <see cref="IPermission"/> interface.
  /// <para>
  /// The action list mask is unused. Thus, BasicPermission is commonly used as
  /// the base class for "named" permissions(ones that contain a name but no
  /// actions list; you either have the named permission or you do not).
  /// </para>
  /// <para>
  /// This class can be used like a role on a role-based authorization model.
  /// </para>
  /// </summary>
  public class BasicPermission : PermissionBase
  {
    #region .ctor
    /// <summary>
    /// Creates a nes BasicPermission with the specified name. Name is the
    /// symbolic name of the permission, such as "exit", "print.Job", or
    /// "topLevelWindow".
    /// </summary>
    /// <param name="name">
    /// The name of the <see cref="BasicPermission"/>.
    /// </param>
    public BasicPermission(string name) : base(name) {
    }
    #endregion

    /// <summary>
    /// Checks if the specified permission is "implied" by this object.
    /// <para>
    /// More specifically, this method returns true if:
    ///     * perm's class is the same as this object's class, and
    ///     * perm's name equals.
    /// </para>
    /// </summary>
    /// <param name="perm">The permission to check against.</param>
    /// <returns>
    /// <c>true</c> if the specified permission is equal to or implied by
    /// this permission; otherwise, false.
    /// </returns>
    public override bool Implies(IPermission perm) {
      return Implies(perm, true);
    }

    /// <summary>
    /// Checks if the specified permission is "implied" by this object.
    /// <para>
    /// More specifically, this method returns true if:
    ///     * perm's class is the same as this object's class, and
    ///     * perm's name equals.
    /// </para>
    /// </summary>
    /// <param name="perm">The permission to check against.</param>
    /// <param name="ignore_case">
    /// A <see cref="Boolean"/> indicating a case-sensitive or insensitive
    /// comparison(<c>true</c> indicates a case-insensitive comparison.)
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified permission is equal to or implied by this
    /// permission; otherwise, false.
    /// </returns>
    public bool Implies(IPermission perm, bool ignore_case) {
      if (perm == null || perm.GetType() != GetType()) {
        return false;
      }

      if (ignore_case) {
        return
          string.Compare(Name, perm.Name,
            StringComparison.InvariantCultureIgnoreCase) == 0;
      }
      return Name == perm.Name;
    }
  }
}
