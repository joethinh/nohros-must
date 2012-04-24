using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros
{
  public class ParameterizedStringPartLiteral : ParameterizedStringPart
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ParameterizedStringPartLiteral"/> class by using the
    /// specified literal text.
    /// </summary>
    /// <param name="literal">
    /// A string that contains the literal value.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="literal"/> is <c>null</c>.
    /// </exception>
    public ParameterizedStringPartLiteral(string literal)
      : base(string.Empty, literal) {
      if (literal == null) {
        throw new ArgumentNullException("literal");
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
      if ((object) other == null) {
        return false;
      }
      return other.value == value;
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
      ParameterizedStringPartLiteral p = obj as ParameterizedStringPartLiteral;
      if ((object) p == null) {
        return false;
      }

      return p.value == value;
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
      return value.GetHashCode();
    }

    /// <summary>
    /// Gets the literal value of the parameterized string part.
    /// </summary>
    /// <remarks>
    /// <see cref="ParameterizedStringPartLiteral"/> is value is immutable.
    /// Attempt to set the value of this property throws an
    /// <see cref="NotSupportedException"/> exception.
    /// </remarks>
    public override string Value {
      set { throw new NotSupportedException("Literal values cannot be modified."); }
    }

    /// <inheritdoc/>
    public override bool IsParameter {
      get { return false; }
    }

    /// <summary>
    /// Gets the string reprentation of this class, that is the literal value
    /// contained by this class.
    /// </summary>
    /// <returns>
    /// The literal value contained by this class.
    /// </returns>
    public override string ToString() {
      return value;
    }
  }
}
