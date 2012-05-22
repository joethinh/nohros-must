using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  public class TokenPrincipalMapperSettings : MustConfiguration,
                                              ITokenPrincipalMapperSettings
  {
    string anonymous_token_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TokenPrincipalMapperSettings"/> class.
    /// </summary>
    public TokenPrincipalMapperSettings() {
      anonymous_token_ = "anonymous";
    }
    #endregion

    #region ITokenPrincipalMapperSettings Members
    /// <inheritdoc/>
    public string AnonymousToken {
      get { return anonymous_token_; }
      protected set { anonymous_token_ = value; }
    }
    #endregion
  }
}
