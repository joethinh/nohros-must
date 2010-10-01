using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// Represents a string value
    /// </summary>
    public class StringValue : Value
    {
        string value_;

        /// <summary>
        /// Initializes a new instance_ of the StringValue class.
        /// </summary>
        public StringValue(string in_value)
            : base(ValueType.TYPE_STRING)
        {
            value_ = in_value;
        }

        public override string GetAsString() {
            return value_;
        }

        public override bool GetAsString(out string out_value)
        {
            out_value = GetAsString();
            return (out_value != null);
        }

        public override IValue DeepCopy()
        {
            return CreateStringValue(value_);
        }

        public override bool Equals(IValue other)
        {
            string lhs, rhs;

            if (other.Type != Type)
                return false;

            return GetAsString(out lhs) && other.GetAsString(out rhs) && lhs == rhs;
        }
    }
}
