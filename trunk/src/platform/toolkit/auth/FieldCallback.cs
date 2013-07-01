using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Underlying security services instantiate and pass a
  /// <see cref="FieldCallback"/> to the hanlde method of a
  /// <see cref="IAuthCallbackHandler"/> to retrieve a form field information
  /// from the current <see cref="System.Web.HttpRequest"/>.
  /// </summary>
  public class FieldCallback: IAuthCallback
  {
    string name_;
    string value_;

    /// <summary>
    /// Initializes a new instance of the <see cref="FieldCallback"/> class by
    /// using the specified field name.
    /// </summary>
    /// <param name="name">The name of the field to retrieve from the
    /// current <see cref="System.Web.HttpRequest"/></param>
    public FieldCallback(string name) {
      name_ = name;
      value_ = null;
    }

    /// <summary>
    /// Gets the value of the field.
    /// </summary>
    public string Value {
      get { return value_; }
      set { value_ = value; }
    }

    /// <summary>
    /// Gets the name of the field.
    /// </summary>
    public string Name {
      get { return name_; }
    }
  }
}
