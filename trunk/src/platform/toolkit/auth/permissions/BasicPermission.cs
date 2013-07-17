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
    /// <summary>
    /// Creates a nes BasicPermission with the specified name. Name is the
    /// symbolic name of the permission, such as  "exit", "print.Job", or
    /// "topLevelWindow".
    /// </summary>
    /// <param name="name">
    /// The name of the <see cref="BasicPermission"/>.
    /// </param>
    public BasicPermission(string name) : base(name, 0) {
      if (name == null) {
        throw new ArgumentNullException("name");
      }

      int i = name.Length;
      if (i == 0) {
        throw new ArgumentException("name");
      }

      name_ = name;
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
    /// <param name="ignoreCase">
    /// A <see cref="Boolean"/> indicating a case-sensitive or insensitive
    /// comparison(<c>true</c> indicates a case-insensitive comparison.)
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified permission is equal to or implied by this
    /// permission; otherwise, false.
    /// </returns>
    public bool Implies(IPermission perm, bool ignoreCase) {
      if (perm == null || !perm.GetType().Equals(this.GetType())) {
        return false;
      }

      var local = perm as BasicPermission;
      return (ignoreCase)
        ? name_.ToLower() == local.name_.ToLower()
        : name_ == local.name_;
    }
  }
}
