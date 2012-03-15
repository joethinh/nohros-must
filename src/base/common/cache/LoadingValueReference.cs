using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Concurrent;

namespace Nohros.Caching
{
  /// <summary>
  /// A <see cref="IValueReference{T}"/> that indicates that the value is
  /// loading.
  /// </summary>
  /// <typeparam name="T">The type of the object that this instance reference.
  /// </typeparam>
  internal class LoadingValueReference<T> : IValueReference<T>
  {
    readonly volatile IValueReference<T> old_value_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingValueReference{T}"/>
    /// with a unset value.
    /// </summary>
    public LoadingValueReference() : this(AbstractCache<T>.Unset) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingValueReference{T}"/>
    /// class by using the specified old value.
    /// </summary>
    /// <param name="old_value"></param>
    public LoadingValueReference(IValueReference<T> old_value) {
      old_value_ = old_value;
    }
    #endregion

    #region IValueReference<T> Members
    public T WaitForValue() {
      // TODO: Check this.
      throw new NotImplementedException();
    }

    public IFuture<T> LoadFuture(string key, CacheLoader<T> loader) {
      long start = Clock.NanoTime;
      T previous_value = old_value_.Value;
    }

    /// <inheritdoc/>
    public bool IsLoading {
      get { return true; }
    }

    /// <inheritdoc/>
    public T Value {
      get { return old_value_.Value; }
    }
    #endregion
  }
}