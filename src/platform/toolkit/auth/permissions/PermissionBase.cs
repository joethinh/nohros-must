using System;
using System.Collections.Generic;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Provides the abstract base class for the <see cref="IPermission"/>
  /// interface.
  /// <para>
  /// This class implements the <see cref="IPermission.Name"/>,
  /// <see cref="IPermission.Actions"/> and the <see cref="Object.GetHashCode"/>
  /// method.
  /// </para>
  /// </summary>
  public abstract class PermissionBase : IPermission
  {
    readonly IEnumerable<string> actions_;
    readonly int hash_;

    /// <summary>
    /// The name of the permission name.
    /// </summary>
    readonly string name_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionBase"/> class
    /// with the specified name.
    /// </summary>
    /// <param name="name">
    /// The name of the permission.
    /// </param>
    /// <remarks>
    /// Permissions objects created by using this constructor have a empty list
    /// of actions.
    /// </remarks>
    protected PermissionBase(string name)
      : this(name, new string[0]) {
      name_ = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionBase"/> class
    /// by using the specified permission name and actions list.
    /// </summary>
    protected PermissionBase(string name, IEnumerable<string> actions) {
      if (name == null) {
        throw new ArgumentNullException("name");
      }

      if (actions == null) {
        throw new ArgumentNullException("actions");
      }

      name_ = name;
      actions_ = actions;
      hash_ = ComputeHash();
    }
    #endregion

    /// <inheritdoc/>
    public abstract bool Implies(IPermission perm);

    /// <summary>
    /// Serves as a hash function for the <see cref="PermisionBase"/> class.
    /// </summary>
    /// <returns>
    /// A hash code for the current permission.
    /// </returns>
    public override int GetHashCode() {
      return hash_;
    }

    /// <summary>
    /// Determines whether this instance of <see cref="IPermission"/> and a
    /// specified object, which must also be a <see cref="IPermission"/>
    /// object, refers to the same permission.
    /// </summary>
    /// <param name="obj">
    /// The object to compare.
    /// </param>
    /// <returns><c>true</c> if the specified object is an
    /// <see cref="IPermission"/> and its name and action list are the same as
    /// this instance; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This class uses the permission name and its action list when comparing
    /// permissions. This method performs an ordinal (case-insensitive and
    /// culture-insensitive) comparison.
    /// </para>
    /// </remarks>
    public override bool Equals(object obj) {
      var permission = obj as IPermission;
      return Equals(permission);
    }

    /// <summary>
    /// Determines whether this instance of <see cref="IPermission"/> and
    /// another specified <see cref="IPermission"/> refers to the same
    /// permission.
    /// </summary>
    /// <param name="perm">
    /// A <see cref="IPermission"/> object.
    /// </param>
    /// <returns>
    /// <c>true</c> if the permission's name and actions list of the
    /// <paramref name="perm"/> object is the same as this instance;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This class uses the permission name and its action list when comparing
    /// permissions. This method performs an ordinal (case-insensitive and
    /// culture-insensitive) comparison.
    /// </para>
    /// </remarks>
    public virtual bool Equals(IPermission perm) {
      if ((object) perm == null) {
        return false;
      }
      return name_ == perm.Name && GetHashCode() == perm.GetHashCode();
    }

    /// <summary>
    /// Gets the name of this permission.
    /// </summary>
    public virtual string Name {
      get { return name_; }
    }

    /// <summary>
    /// Gets the actions bitmask that tells the actions that are permited for
    /// this object.
    /// </summary>
    public virtual IEnumerable<string> Actions {
      get { return actions_; }
    }

    int ComputeHash() {
      int hash = 17;
      hash = hash*23 + name_.GetHashCode();
      foreach (string action in actions_) {
        hash = hash*23 + action.GetHashCode();
      }
      return hash;
    }
  }
}
