using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros
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
    /// <exception cref="ArgumentNullException"><paramref name="literal_text"/> is null</exception>
    public ParameterizedStringPart(string literal_text) {
      if (literal_text == null)
        throw new ArgumentNullException("literal_text");

      parameter_name_ = string.Empty;
      parameter_value_ = literal_text;
      is_parameter_ = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterizedStringPart"/> class, using the provided parameter
    /// name and value.
    /// </summary>
    /// <param name="parameter_name">The name of the parameter.</param>
    /// <param name="parameter_value">The value of the parameter. This could be null.</param>
    /// <exception cref="ArgumentNullException"><paramref name="parameter_name"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="parameter_name"/>is a empty string.</exception>
    public ParameterizedStringPart(string parameter_name, string parameter_value) {
      if (parameter_name == null)
        throw new ArgumentNullException("parameter_name");

      if (parameter_name.Length == 0)
        throw new ArgumentException("parameter_name");

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
      if (((object)other) != null && is_parameter_ == other.is_parameter_) {
        return (is_parameter_) ?
            string.Equals(parameter_name_, other.parameter_name_, StringComparison.OrdinalIgnoreCase) :
            string.Equals(parameter_value_, other.parameter_value_, StringComparison.OrdinalIgnoreCase);
      }
      return false;
    }

    /// <summary>
    /// Gets a value that indicates whether the provided object is equal to this object.
    /// </summary>
    /// <param name="obj">An object that can be cast to a ParameterizedStringPart object.</param>
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
      return (((((object)part1) == null) && (((object)part2) == null)) || part1.Equals(part2));
    }

    /// <summary>
    /// Provides an inequality operator (!=) for comparing two ParameterizedStringPart object.
    /// </summary>
    /// <param name="part1">A ParameterizedStringPart object.</param>
    /// <param name="part2">A ParameterizedStringPart object.</param>
    /// <returns>true if the two objects are not equals; otherwise, false.</returns>
    public static bool operator !=(ParameterizedStringPart part1, ParameterizedStringPart part2) {
      return !(part1 == part2);
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
      set { parameter_value_ = value; }
    }

    /// <summary>
    /// Gets a value that indicates whether the string is a parameter.
    /// </summary>
    public bool IsParameter {
      get { return is_parameter_; }
    }
  }
}
