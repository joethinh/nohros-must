using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros
{
  /// <summary>
  /// A <see cref="ISupplier{T}"/> that always returns the same instance.
  /// </summary>
  /// <remarks><see cref="SupplierOfInstance{T}"/> is similar to
  /// <see cref="MemoizingSupplier{T}"/>, but instead to create a instance it
  /// uses the instance that is passed to it on the constructor.
  /// </remarks>
  internal class SupplierOfInstance<T> : ISupplier<T>
  {
    readonly T instance_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SupplierOfInstance{T}"/>
    /// class by using the specified <paramref name="instance"/> object.
    /// </summary>
    /// <param name="instance">The object that will be supplied on every
    /// call to <see cref="Supply"/></param>
    public SupplierOfInstance(T instance) {
      instance_ = instance;
    }
    #endregion

    /// <inheritdoc/>
    public T Supply() {
      return instance_;
    }
  }
}
