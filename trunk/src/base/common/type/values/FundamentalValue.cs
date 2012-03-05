using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Nohros
{
    /// <summary>
    /// Represents the simple fundamental types of values.
    /// </summary>
    public class FundamentalValue : Value
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
            : base(ValueType.TYPE_BOOLEAN)
        {
            in_value_ = new FundamentalValue_();
            in_value_.boolean_value_ = in_value;
        }

        /// <summary>
        /// Initializes a new instance_ of the FundamentalCalss by using the specified boolean value.
        /// </summary>
        /// <param name="in_value">The underlying value</param>
        public FundamentalValue(int in_value)
            : base(ValueType.TYPE_INTEGER)
        {
            in_value_ = new FundamentalValue_();
            in_value_.integer_value_ = in_value;
        }

        /// <summary>
        /// Initializes a new instance_ of the FundamentalCalss by using the specified boolean value.
        /// </summary>
        /// <param name="in_value">The underlying value</param>
        public FundamentalValue(double in_value)
            : base(ValueType.TYPE_REAL)
        {
            in_value_ = new FundamentalValue_();
            in_value_.real_value_ = in_value;
        }
        #endregion

        public override bool GetAsBoolean(out bool out_value)
        {
            if (IsType(ValueType.TYPE_BOOLEAN))
                out_value = in_value_.boolean_value_;
            else
                out_value = default(bool);

            return IsType(ValueType.TYPE_BOOLEAN);
        }

        public override bool GetAsInteger(out int out_value)
        {
            if (IsType(ValueType.TYPE_INTEGER))
                out_value = in_value_.integer_value_;
            else
                out_value = default(int);

            return IsType(ValueType.TYPE_INTEGER);
        }

        public override bool GetAsReal(out double out_value)
        {
            if (IsType(ValueType.TYPE_REAL))
                out_value = in_value_.real_value_;
            else
                out_value = default(double);

            return IsType(ValueType.TYPE_REAL);
        }

        public override IValue DeepCopy()
        {
            switch(ValueType) {
                case ValueType.TYPE_BOOLEAN:
                    return CreateBooleanValue(in_value_.boolean_value_);

                case ValueType.TYPE_INTEGER:
                    return CreateIntegerValue(in_value_.integer_value_);

                case ValueType.TYPE_REAL:
                    return CreateRealValue(in_value_.real_value_);

                default:
                    return null;
            }
        }

        public override bool Equals(IValue other)
        {
            if (other.ValueType != ValueType)
                return false;

            switch(ValueType) {
                case ValueType.TYPE_BOOLEAN:
                    {
                        bool lhs, rhs;
                        return GetAsBoolean(out lhs) && other.GetAsBoolean(out rhs) && lhs == rhs;
                    }

                case ValueType.TYPE_INTEGER:
                    {
                        int lhs, rhs;
                        return GetAsInteger(out lhs) && other.GetAsInteger(out rhs) && lhs == rhs;
                    }

                case ValueType.TYPE_REAL:
                    {
                        double lhs, rhs;
                        return GetAsReal(out lhs) && other.GetAsReal(out rhs) && lhs == rhs;
                    }

                default:
                    return false;
            }
        }
    }
}
