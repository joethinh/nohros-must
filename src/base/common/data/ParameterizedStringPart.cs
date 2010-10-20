using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// Represents an embedded parameter in a <see cref="ParameterizedString"/> object.
    /// </summary>
    public class ParameterizedStringPart
    {
        string parameter_name_;
        string parameter_value_;
        bool is_parameter_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the ParameterizedStringPart class by using the specified literal text.
        /// </summary>
        /// <param name="literal_text">A string that contains the parameterized string part.</param>
        public ParameterizedStringPart(string literal_text) {
            parameter_name_ = string.Empty;
            parameter_value_ = literal_text;
            is_parameter_ = false;
        }

        /// <summary>
        /// Initializes a new instance of the ParameterizedStringPart class, using the provided parameter name and
        /// value.
        /// </summary>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="parameter_value">The value of the parameter. This could be null.</param>
        public ParameterizedStringPart(string parameter_name, string parameter_value) {
            parameter_name_ = parameter_name;
            parameter_value_ = parameter_value;
            is_parameter_ = true;
        }
        #endregion

        /// <summary>
        /// Gets a value that indicates whether the provided object is equal to this object.
        /// </summary>
        /// <param name="other">A ParameterizedStringPart object.</param>
        /// <returns>true if the provided object is equals to this object; otherwise, false.</returns>
        public bool Equals(ParameterizedStringPart other) {
            return ((is_parameter_ == other.is_parameter_) && string.Equals(parameter_name_, other.parameter_name_, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets a value that indicates whether the provided object is equal to this object.
        /// </summary>
        /// <param name="other">An object that can be cast to a ParameterizedStringPart object.</param>
        /// <returns>true if the provided object is equals to this object; otherwise, false.</returns>
        public override bool Equals(object obj) {
            return ((obj is ParameterizedStringPart) && Equals((ParameterizedStringPart)obj));
        }

        /// <summary>
        /// Provides an equality operator (=) for comparing two ParameterizedStringPart object.
        /// </summary>
        /// <param name="part1">A ParameterizedStringPart object.</param>
        /// <param name="part2">A ParameterizedStringPart object.</param>
        /// <returns>true if the two objects are equals; otherwise, false.</returns>
        public static bool operator ==(ParameterizedStringPart part1, ParameterizedStringPart part2) {
            return part1.Equals(part2);
        }

        /// <summary>
        /// Provides an inequality operator (!=) for comparing two ParameterizedStringPart object.
        /// </summary>
        /// <param name="part1">A ParameterizedStringPart object.</param>
        /// <param name="part2">A ParameterizedStringPart object.</param>
        /// <returns>true if the two objects are not equals; otherwise, false.</returns>
        public static bool operator !=(ParameterizedStringPart part1, ParameterizedStringPart part2) {
            return !part1.Equals(part2);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        /// <remarks>
        /// This ethod overrides the <see cref="object.GetHashCode()"/> and more complete documentation might
        /// be available in that topic.
        /// </remarks>
        public override int GetHashCode() {
            return string.Concat(parameter_name_, parameter_value_).GetHashCode();
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string ParameterName {
            get { return parameter_name_; }
        }

        /// <summary>
        /// Gets the literal string for this parameterized part.
        /// </summary>
        public string LiteralValue {
            get { return parameter_value_; }
        }

        /// <summary>
        /// Gets a value that indicates whether the string is a parameter.
        /// </summary>
        public bool IsParameter {
            get { return is_parameter_; }
        }
    }
}
