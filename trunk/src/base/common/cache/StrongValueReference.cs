using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching
{
  /// <summary>
  /// References a string value.
  /// </summary>
  /// <typeparam name="T">
  /// The type of the value that is referenced.
  /// </typeparam>
  internal class StrongValueReference<T> : IValueReference<T>
  {
    readonly T referent_;

    /// <summary>
    /// Initializes a new instance of the <see cref="StrongValueReference"/>
    /// class by using the specified referent value.
    /// </summary>
    /// <param name="referent"></param>
    public StrongValueReference(T referent) {
      referent_ = referent;
    }

    /// <inheritdoc/>
    public T Value {
      get { return referent_; }
    }

    /// <inheritdoc/>
    public bool IsLoading {
      get { return false; }
    }

    /// <inheritdoc/>
    public bool IsActive {
      get { return true; }
    }

    /// <inheritdoc/>
    public T WaitForValue() {
      return Value;
    }
  }
}
