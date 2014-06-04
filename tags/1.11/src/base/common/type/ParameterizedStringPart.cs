using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros
{
  /// <summary>
  /// Represents an embedded parameter in a <see cref="ParameterizedString"/>
  /// object.
  /// </summary>
  public abstract class ParameterizedStringPart
  {
    protected string value;
    protected readonly string name;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterizedStringPart"/>
    /// class, using the provided parameter name and value.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter.
    /// </param>
    /// <param name="value">
    /// The value of the parameter. This could be <c>null</c>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> is <c>null</c>.
    /// </exception>
    protected ParameterizedStringPart(string name, string value) {
      if (name == null)
        throw new ArgumentNullException("name");

      this.name = name;
      this.value = value;
    }
    #endregion

    /// <summary>
    /// Gets the name of the parameter.
    /// </summary>
    public string Name {
      get { return name; }
    }

    /// <summary>
    /// Gets the literal string for this parameterized part.
    /// </summary>
    public virtual string Value {
      get { return value; }
      set { this.value = value; }
    }

    /// <summary>
    /// Gets a value that indicates whether the string is a parameter.
    /// </summary>
    public abstract bool IsParameter { get; }
  }
}
