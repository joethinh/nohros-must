using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Underlying security services instantiate and pass a
  /// <see cref="FieldCallback"/> to the handle method of a
  /// <see cref="IAuthCallbackHandler"/> to retrieve a form field information
  /// from the current <see cref="System.Web.HttpRequest"/>.
  /// </summary>
  public class FieldCallback : IAuthCallback
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldCallback"/> class by
    /// using the specified field name.
    /// </summary>
    /// <param name="name">
    /// The name of the field to retrieve from the current
    /// <see cref="System.Web.HttpRequest"/>.
    /// </param>
    public FieldCallback(string name) {
      Name = name;
      Value = null;
    }
    #endregion

    /// <summary>
    /// Gets the value of the field.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets the name of the field.
    /// </summary>
    public string Name { get; private set; }
  }
}
