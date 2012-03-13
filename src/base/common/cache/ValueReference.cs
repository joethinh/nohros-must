using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching
{
  /// <summary>
  /// Placeholder. Indicates that the value hasn't been set yet.
  /// </summary>
  internal sealed class UnsetValueReference : IValueReference<object>
  {
    internal static readonly IValueReference<object> UNSET;

    #region .ctor
    static UnsetValueReference() {
      UNSET = new UnsetValueReference();
    }
    #endregion

    #region IValueReference<object> Members
    /// <inheritdoc/>
    public bool IsLoading {
      get { return false; }
    }
    #endregion

    /// <inheritdoc/>
    public object WaitForValue() {
      return null;
    }

    /// <inheritdoc/>
    public object Value {
      get { return null; }
    }
  }
}