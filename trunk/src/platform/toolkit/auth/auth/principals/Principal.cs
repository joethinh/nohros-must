using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// A implementation of the <see cref="IPrincipal"/> class that uses a
  /// generic <typeparamref name="T"/> as the principal identifier.
  /// </summary>
  /// <typeparam name="T">
  /// The type of the object that is used as the principal identifier.
  /// </typeparam>
  public class Principal<T> : IPrincipal
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="IPrincipal"/> class
    /// by using the given <typeparamref name="T"/> as principal identifier.
    /// </summary>
    /// <param name="id">
    /// A <typeparamref name="T"/> object that is used to identify the
    /// princiapal within a context.
    /// </param>
    public Principal(T id) {
      Id = id;
    }
    #endregion

    /// <inheritdoc/>
    public override int GetHashCode() {
      return Id.GetHashCode();
    }

    object IPrincipal.Id {
      get { return Id.ToString(); }
    }

    /// <inheritdoc/>
    public bool Equals(IPrincipal principal) {
      var p = principal as Principal<T>;
      return Equals(p);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) {
      var p = obj as Principal<T>;
      return Equals(p);
    }

    /// <inheritdoc/>
    public override string ToString() {
      return Id.ToString();
    }

    /// <inheritdoc/>
    protected bool Equals(Principal<T> principal) {
      if ((object) principal == null) {
        return false;
      }
      return (principal.Id.Equals(Id));
    }

    /// <inheritdoc/>
    public T Id { get; private set; }
  }
}
