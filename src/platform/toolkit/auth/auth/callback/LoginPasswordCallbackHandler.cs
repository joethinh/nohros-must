using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// A implementation of the <see cref="IAuthCallbackHandler"/> that is
  /// capable to handle callbacks of the type <see cref="FieldCallback"/> which
  /// name is "login" or "password".
  /// </summary>
  public class LoginPasswordCallbackHandler : IAuthCallbackHandler
  {
    public const string kLoginFieldName = "login";
    public const string kPasswordFieldName = "password";

    readonly string login_;
    readonly string password_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="LoginPasswordCallbackHandler"/> class by using the given
    /// login and password.
    /// </summary>
    /// <param name="login">
    /// The value that should be assocaited with the
    /// <see cref="LoginPasswordCallbackHandler.kLoginFieldName"/> field.
    /// </param>
    /// <param name="password">
    /// The value that should be assocaited with the
    /// <see cref="LoginPasswordCallbackHandler.kPasswordFieldName"/> field.
    /// </param>
    public LoginPasswordCallbackHandler(string login, string password) {
      login_ = login;
      password_ = password;
    }
    #endregion

    /// <inheritdoc/>
    public void Handle(IAuthCallback[] callbacks) {
      foreach (var callback in callbacks) {
        var field = callback as FieldCallback;
        if (field == null) {
          throw new NotSupportedException();
        }

        switch (field.Name) {
          case kLoginFieldName:
            field.Value = login_;
            break;

          case kPasswordFieldName:
            field.Value = password_;
            break;

          default:
            throw new NotSupportedException();
        }
      }
    }
  }
}
