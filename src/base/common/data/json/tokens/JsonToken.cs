using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data.Json
{
  /// <summary>
  /// Provides skeletal implementation of the <see cref="IJsonToken{T}"/>
  /// interface to redecue the effort required to implement that interface.
  /// </summary>
  /// <typeparam name="T">
  /// The type of the underlying value.
  /// </typeparam>
  public abstract class JsonToken<T> : IJsonToken<T>
  {
    protected readonly T value;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonToken{T}"/> class
    /// using the specified value.
    /// </summary>
    /// <param name="value">
    /// The underlying value.
    /// </param>
    protected JsonToken(T value) {
      this.value = value;
    }
    #endregion

    #region IJsonToken<T> Members
    /// <inheritdoc/>
    public abstract string AsJson();

    /// <inheritdoc/>
    public virtual T Value {
      get { return value; }
    }
    #endregion
  }
}
