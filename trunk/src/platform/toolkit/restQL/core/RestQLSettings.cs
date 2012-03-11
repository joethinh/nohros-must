using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// The application settings.
  /// </summary>
  public class RestQLSettings : NohrosConfiguration
  {
    string anonymous_token_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="RestQLSettings"/> class.
    /// </summary>
    public RestQLSettings() {
      anonymous_token_ = "anonymous";
    }
    #endregion

    /// <summary>
    /// Gets the token defined by for the anonymous user.
    /// </summary>
    public string AnonymousToken {
      get { return anonymous_token_; }
      private set { anonymous_token_ = value; }
    }
  }
}
