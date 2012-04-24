using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Resources;

namespace Nohros
{
  public class ParameterizedStringPartParameter : ParameterizedStringPart
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ParameterizedStringPartParameter"/> class by using the
    /// specified parameter name.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the paramter within a parameterized
    /// string.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> is <c>null</c>.
    /// </exception>
    public ParameterizedStringPartParameter(string name)
      : base(name, string.Empty) {
      if (name == null) {
        throw new ArgumentNullException("name");
      }
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ParameterizedStringPartParameter"/> class by using the
    /// specified parameter name and value.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the parameter within a parameterized
    /// string.
    /// </param>
    /// <param name="value">
    /// The paramter's value. This value will replace the parameteron the final
    /// parameterized string.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="name"/> is a empty string or a sequence of spaces.
    /// </exception>
    public ParameterizedStringPartParameter(string name, string value)
      : base(name.Trim(), value) {
      if (name.Trim().Length == 0) {
        throw new ArgumentException(
          StringResources.Argument_EmptyStringOrSpaceSequence);
      }
    }
    #endregion

    /// <summary>
    /// Gets a value that indicates whether the provided object is equal to
    /// this object.
    /// </summary>
    /// <param name="other">
    /// A  <see cref="ParameterizedStringPartLiteral"/> object.
    /// </param>
    /// <returns>
    /// <c>true</c> if the provided object is equals to this object; otherwise,
    /// <c>false</c>.</returns>
    public bool Equals(ParameterizedStringPartLiteral other) {
      if ((object)other == null) {
        return false;
      }
      return other.Name == name;
    }

    /// <summary>
    /// Gets a value that indicates whether the provided object is equal to
    /// this object.
    /// </summary>
    /// <param name="obj">
    /// An object that can be cast to a <see cref="ParameterizedStringPartLiteral"/>
    /// object.
    /// </param>
    /// <returns>
    /// <c>true</c> if the provided object is equals to this object; otherwise,
    /// <c>false</c>.</returns>
    public override bool Equals(object obj) {
      ParameterizedStringPartParameter p =
        obj as ParameterizedStringPartParameter;

      if ((object)p == null) {
        return false;
      }

      return p.name == name;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>
    /// A 32-bit signed integer that is the hash code for this instance.
    /// </returns>
    /// <remarks>
    /// This method overrides the <see cref="object.GetHashCode()"/> and more
    /// complete documentation might be available in that topic.
    /// </remarks>
    public override int GetHashCode() {
      // value is immutable, we can use it as hash code.
      return name.GetHashCode();
    }

    /// <inheritdoc/>
    public override bool IsParameter {
      get { return true; }
    }
  }
}
