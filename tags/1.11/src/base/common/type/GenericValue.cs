using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros
{
  /// <summary>
  /// Represents a Generic class value.
  /// </summary>
  public class GenericValue<T>: Value where T: class
  {
    T value_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance_ of the Generic&lt;T&gt; class by using the specified value.
    /// </summary>
    /// <param name="in_value">The underlying generic value. </param>
    public GenericValue(T in_value)
      : base(ValueType.Generic) {
      value_ = in_value;
    }
    #endregion

    /// <summary>
    /// Creates a new instance of the <see cref="Generic&lt;T&gt;"/> class, using the specified
    /// <typeparamref name="T"/> object.
    /// </summary>
    /// <param name="t"></param>
    /// <returns>A instance of the <see cref="Generic&lt;T&gt;"/> class with underlying type <typeparamref name="T"/></returns>
    static GenericValue<T> FromT(T t) {
      GenericValue<T> gen_t = new GenericValue<T>(t);
      return gen_t;
    }

    /// <summary>
    /// Determines whether the specified <see cref="Value"/> is equal to the current <see cref="Value"/>
    /// </summary>
    /// <param name="other">The <see cref="Value"/> to compare with the current <see cref="Value"/></param>
    /// <returns>true if the specified <see cref="Value"/> is equals to the current <see cref="Value"/>; otherwise, false.</returns>
    public override bool Equals(IValue other) {
      return value_.Equals(other);
    }

    /// <summary>
    /// Gets the underlying value for this Value.
    /// </summary>
    public T TValue {
      get { return value_; }
    }
  }
}
