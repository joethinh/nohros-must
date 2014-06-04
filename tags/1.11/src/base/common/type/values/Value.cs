using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Resources;

namespace Nohros
{
  /// <summary>
  /// The <see cref="Value"/> class is the base of class for values. A
  /// <see cref="IValue"/> can be instantiated via Create*Value factory methods,
  /// or by directly creating instances of the subclasses.
  /// </summary>
  /// <seealso cref="IValue"/>
  public class Value: IValue
  {
    ValueType type_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the Value class.
    /// </summary>
    Value() { }

    /// <summary>
    /// This isn't safe for end-users (they should use the Create*Value()
    /// static method above), but it's useful for subclasses.
    /// </summary>
    /// <param name="type">The type of the Value</param>
    protected Value(ValueType type) {
      type_ = type;
    }
    #endregion

    /// <summary>
    /// Convenience method for creating a <see cref="IValue"/> object of type
    /// <see cref="ValueType.TYPE_NULL"/> without thinking about which class
    /// implements it.
    /// </summary>
    /// <returns>A <see cref="IValue"/> object which <see cref="ValueType"/> is
    /// equals to <see cref="ValueType.TYPE_NULL"/></returns>
    public static IValue CreateNullValue() {
      return new Value(ValueType.NullValue);
    }

    /// <summary>
    /// Convenience method for creating a <see cref="IValue"/> object of type
    /// <see cref="ValueType.TYPE_BOOLEAN"/> without thinking about which class
    /// implements it.
    /// </summary>
    /// <returns>A <see cref="IValue"/> object which <see cref="ValueType"/> is
    /// equals to <see cref="ValueType.TYPE_BOOLEAN"/></returns>
    public static IValue CreateBooleanValue(bool in_value) {
      return new FundamentalValue(in_value);
    }

    /// <summary>
    /// Convenience method for creating a <see cref="IValue"/> object of type
    /// <see cref="ValueType.TYPE_INTEGER"/> without thinking about which class
    /// implements it.
    /// </summary>
    /// <returns>A <see cref="IValue"/> object which <see cref="ValueType"/> is
    /// equals to <see cref="ValueType.TYPE_INTEGER"/></returns>
    public static IValue CreateIntegerValue(int in_value) {
      return new FundamentalValue(in_value);
    }

    /// <summary>
    /// Convenience method for creating a <see cref="IValue"/> object of type
    /// <see cref="ValueType.TYPE_REAL"/> without thinking about which class
    /// implements it.
    /// </summary>
    /// <returns>A <see cref="IValue"/> object which <see cref="ValueType"/> is
    /// equals to <see cref="ValueType.TYPE_REAL"/></returns>
    public static IValue CreateRealValue(double in_value) {
      return new FundamentalValue(in_value);
    }

    /// <summary>
    /// Convenience method for creating <see cref="IValue"/> of type
    /// <see cref="ValueType.TYPE_STRING"/> without thinking about which class
    /// implements it.
    /// </summary>
    /// <returns>A <see cref="IValue"/> object which <see cref="ValueType"/> is
    /// equals to <see cref="ValueType.TYPE_STRING"/></returns>
    public static IValue CreateStringValue(string in_value) {
      return new StringValue(in_value);
    }

    /// <summary>
    /// Convenience method for creating <see cref="IValue"/> of type
    /// <see cref="ValueType.TYPE_GENERIC"/> without thinking about which
    /// class implements it.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="in_value"/>
    /// parameter.</typeparam>
    /// <param name="in_value">The underlying <typeparamref name="T"/>value.
    /// </param>
    /// <returns>A <see cref="IValue"/> object which <see cref="ValueType"/> is
    /// equals to <see cref="ValueType.TYPE_GENERIC"/>.</returns>
    public static IValue CreateGenericValue<T>(T in_value) where T: class {
      return new GenericValue<T>(in_value);
    }

    #region bool GetAs[...](out ...) methods
    /// <inheritdoc/>
    public virtual bool GetAsBoolean(out bool out_value) {
      out_value = default(bool);
      return false;
    }

    /// <inheritdoc/>
    public virtual bool GetAsInteger(out int out_value) {
      out_value = default(int);
      return false;
    }

    /// <inheritdoc/>
    public virtual bool GetAsReal(out double out_value) {
      out_value = default(double);
      return false;
    }

    /// <inheritdoc/>
    public virtual bool GetAsString(out string out_value) {
      out_value = default(string);
      return false;
    }
    #endregion

    #region [T] GetAs[...]() methods
    /// <inheritdoc/>
    public virtual bool GetAsBoolean() {
      Thrower.ThrowInvalidCastException("Value", "boolean");
      return false;
    }

    /// <inheritdoc/>
    public virtual int GetAsInteger() {
      Thrower.ThrowInvalidCastException("Value", "int");
      return 0;
    }

    /// <inheritdoc/>
    public virtual double GetAsReal() {
      Thrower.ThrowInvalidCastException("Value", "real");
      return 0.0;
    }

    /// <inheritdoc/>
    public virtual string GetAsString() {
      throw new InvalidCastException(
        string.Format(StringResources.InvalidCast_FromTo, "Value", "string"));
    }
    #endregion

    /// <summary>
    /// Creates a deep copy of the entire <see cref="Value"/> tree.
    /// </summary>
    /// <returns>A deep copy of the entire value tree.</returns>
    /// <remarks>This method should only be getting called for value whose
    /// type is <see cref="ValueType.TYPE_NULL"/>; all
    /// subclasses need to provide their own implementation.</remarks>
    public virtual IValue DeepCopy() {
      if (IsType(ValueType.NullValue))
        return CreateNullValue();
      throw new NotImplementedException();
    }

    /// <summary>
    /// Compares if two <see cref="Value"/> objects have equal contents.
    /// </summary>
    /// <returns><c>true</c> if this instance have equals contents of
    /// <paramref name="other"/>.</returns>
    /// <remarks>This method should only be getting called for values
    /// whose type is <see cref="ValueType.TYPE_NULL"/>; all
    /// subclasses need to provide their own implementation.</remarks>
    public virtual bool Equals(IValue other) {
      if (IsType(ValueType.NullValue))
        return other.IsType(ValueType.NullValue);
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets a value indicating whether the current object represents a given
    /// type or not.
    /// </summary>
    /// <returns><c>true</c> if the current object represents a given type.
    /// </returns>
    public bool IsType(ValueType type) {
      return type_ == type;
    }

    /// <summary>
    /// Gets the type of the value stored by the current <see cref="Value"/>
    /// object. Each type will be implemented by only one subclass of
    /// <see cref="Value"/> Value, so it's safe to use the
    /// <see cref="ValueType"/> to determine whether you can cast from
    /// <see cref="Value"/> to (Implementating Class)[*]. Also, a
    /// <see cref="Value"/> Value object never changes its type after
    /// construction.
    /// </summary>
    public ValueType ValueType {
      get { return type_; }
    }
  }
}
