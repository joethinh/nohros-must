using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching
{
  /// <summary>
  /// Placeholder. Indicates that the value hasn't been set yet.
  /// </summary>
  internal sealed class UnsetValueReference<T> : IValueReference<T>
  {
    internal static readonly IValueReference<T> UNSET;

    #region .ctor
    static UnsetValueReference() {
      UNSET = new UnsetValueReference<T>();
    }

    UnsetValueReference() { }
    #endregion

    #region IValueReference<object> Members

    /// <inheritdoc/>
    public bool IsLoading {
      get { return false; }
    }

    /// <inheritdoc/>
    T IValueReference<T>.Value {
      get { return default(T); }
    }

    /// <inheritdoc/>
    T IValueReference<T>.WaitForValue() {
      return default(T);
    }

    /// <inheritdoc/>
    public bool IsActive {
      get { return false; }
    }
    #endregion
  }
}