using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.RestQL
{
  public class TokenPrincipalMapperSettings : ITokenPrincipalMapperSettings
  {
    string anonymous_token_;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TokenPrincipalMapperSettings"/> class.
    /// </summary>
    public TokenPrincipalMapperSettings() {
      anonymous_token_ = "anonymous";
    }

    /// <inheritdoc/>
    public string AnonymousToken {
      get { return anonymous_token_; }
      protected set { anonymous_token_ = value; }
    }
  }
}
