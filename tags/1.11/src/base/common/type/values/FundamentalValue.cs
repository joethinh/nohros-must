using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Nohros
{
  /// <summary>
  /// Represents the simple fundamental types of values.
  /// </summary>
  public class FundamentalValue: Value
  {
    [StructLayout(LayoutKind.Explicit)]
    struct FundamentalValue_
    {
      [FieldOffset(0)]
      public bool boolean_value_;
      [FieldOffset(0)]
      public int integer_value_;
      [FieldOffset(0)]
      public double real_value_;
    }

    FundamentalValue_ in_value_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance_ of the FundamentalCalss by using the specified boolean value.
    /// </summary>
    /// <param name="in_value">The underlying value</param>
    public FundamentalValue(bool in_value)
      : base(ValueType.Boolean) {
      in_value_ = new FundamentalValue_();
      in_value_.boolean_value_ = in_value;
    }

    /// <summary>
    /// Initializes a new instance_ of the FundamentalCalss by using the specified boolean value.
    /// </summary>
    /// <param name="in_value">The underlying value</param>
    public FundamentalValue(int in_value)
      : base(ValueType.Integer) {
      in_value_ = new FundamentalValue_();
      in_value_.integer_value_ = in_value;
    }

    /// <summary>
    /// Initializes a new instance_ of the FundamentalCalss by using the specified boolean value.
    /// </summary>
    /// <param name="in_value">The underlying value</param>
    public FundamentalValue(double in_value)
      : base(ValueType.Real) {
      in_value_ = new FundamentalValue_();
      in_value_.real_value_ = in_value;
    }
    #endregion

    /// <inheritdoc/>
    public override bool GetAsBoolean(out bool out_value) {
      if (IsType(ValueType.Boolean))
        out_value = in_value_.boolean_value_;
      else
        out_value = default(bool);

      return IsType(ValueType.Boolean);
    }

    /// <inheritdoc/>
    public override bool GetAsInteger(out int out_value) {
      if (IsType(ValueType.Integer))
        out_value = in_value_.integer_value_;
      else
        out_value = default(int);

      return IsType(ValueType.Integer);
    }

    /// <inheritdoc/>
    public override bool GetAsReal(out double out_value) {
      if (IsType(ValueType.Real))
        out_value = in_value_.real_value_;
      else
        out_value = default(double);

      return IsType(ValueType.Real);
    }

    /// <inheritdoc/>
    public override bool GetAsBoolean() {
      bool value;
      if(!GetAsBoolean(out value)) {
        // TODO: Add a message to the exception
        throw new InvalidCastException();
      }
      return value;
    }

    /// <inheritdoc/>
    public override int GetAsInteger() {
      int value;
      if (!GetAsInteger(out value)) {
        // TODO: Add a message to the exception
        throw new InvalidCastException();
      }
      return value;
    }

    /// <inheritdoc/>
    public override double GetAsReal() {
      double value;
      if (!GetAsReal(out value)) {
        // TODO: Add a message to the exception
        throw new InvalidCastException();
      }
      return value;
    }

    public override IValue DeepCopy() {
      switch (ValueType) {
        case ValueType.Boolean:
          return CreateBooleanValue(in_value_.boolean_value_);

        case ValueType.Integer:
          return CreateIntegerValue(in_value_.integer_value_);

        case ValueType.Real:
          return CreateRealValue(in_value_.real_value_);

        default:
          return null;
      }
    }

    public override bool Equals(IValue other) {
      if (other.ValueType != ValueType)
        return false;

      switch (ValueType) {
        case ValueType.Boolean: {
            bool lhs, rhs;
            return GetAsBoolean(out lhs) && other.GetAsBoolean(out rhs) && lhs == rhs;
          }

        case ValueType.Integer: {
            int lhs, rhs;
            return GetAsInteger(out lhs) && other.GetAsInteger(out rhs) && lhs == rhs;
          }

        case ValueType.Real: {
            double lhs, rhs;
            return GetAsReal(out lhs) && other.GetAsReal(out rhs) && lhs == rhs;
          }

        default:
          return false;
      }
    }
  }
}
