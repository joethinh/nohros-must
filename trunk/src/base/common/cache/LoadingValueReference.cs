using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Concurrent;
using System.Diagnostics;

namespace Nohros.Caching
{
  /// <summary>
  /// A <see cref="IValueReference{T}"/> that indicates that the value is
  /// loading.
  /// </summary>
  /// <typeparam name="T">The type of the object that this instance reference.
  /// </typeparam>
  internal class LoadingValueReference<T> : AbstractFuture<T>,
                                            IValueReference<T>
  {
    readonly IValueReference<T> old_value_;
    readonly Stopwatch stopwatch_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingValueReference{T}"/>
    /// with a unset value.
    /// </summary>
    public LoadingValueReference() : this(UnsetValueReference<T>.UNSET) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingValueReference{T}"/>
    /// class by using the specified old value.
    /// </summary>
    /// <param name="old_value"></param>
    public LoadingValueReference(IValueReference<T> old_value) {
      old_value_ = old_value;
      stopwatch_ = new Stopwatch();
    }
    #endregion

    public T WaitForValue() {
      return Uninterruptibles.GetUninterruptibly(this);
    }

    /// <inheritdoc/>
    public bool IsLoading {
      get { return true; }
    }

    public bool IsActive {
      get { return old_value_.IsActive; }
    }

    /// <inheritdoc/>
    public T Value {
      get { return old_value_.Value; }
    }

    public IFuture<T> LoadFuture(string key, CacheLoader<T> loader) {
      stopwatch_.Start();
      T previous_value = old_value_.Value;
      try {
        // If the T is a value type and its value is default(T) it will never
        // be reloaded and always loaded from the cache loader. We could
        // make the parameter T nullable, but this makes the code much more
        // complicated and we do not want that.
        if (typeof (T).IsValueType || (object) previous_value == null) {
          T new_value = loader.Load(key);
          return Set(new_value) ? this : Futures.ImmediateFuture(new_value);
        } else {
          IFuture<T> new_value_future = loader.Reload(key, previous_value);
          return new_value_future ?? Futures.ImmediateFuture(default(T));
        }
      } catch (Exception exception) {
        return SetException(exception)
          ? this
          : Futures.ImmediateFailedFuture<T>(exception);
      }
    }

    /// <summary>
    /// Sets the value of this loading value.
    /// </summary>
    /// <param name="value">The value this reference should hold.</param>
    /// <returns><c>true</c> if the value is successfully set, or <c>false</c>
    /// if the value has already been set or if the loading was cancelled.
    /// </returns>
    public new bool Set(T value) {
      return base.Set(value);
    }

    /// <summary>
    /// Sets the loading value to having failed with the given exception.
    /// </summary>
    /// <param name="exception">
    /// The exception this reference  should hold.
    /// </param>
    /// <returns><c>true</c> if the exception is succesfully set, <c>false</c>
    /// if the exception has already been set or if the loading was cancelled.
    /// </returns>
    /// <remarks>The exception will be wrapped in a
    /// <see cref="ExecutionException"/> and thrown from the get methods.
    /// </remarks>
    public new bool SetException(Exception exception) {
      return base.SetException(exception);
    }

    /// <summary>
    /// Gets the total elapsed time since the future starts computing its
    /// value.
    /// </summary>
    public TimeSpan Elapsed {
      get { return stopwatch_.Elapsed; }
    }

    /// <summary>
    /// Gets the total elapsed time since the future starts computing its
    /// value in nanoseconds.
    /// </summary>
    public long ElapsedNanos {
      get { return Elapsed.Ticks*100; }
    }

    public IValueReference<T> OldValue {
      get { return old_value_; }
    }
  }
}
