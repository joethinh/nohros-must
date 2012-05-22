using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Nohros.Toolkit.RestQL
{
  public partial class Settings : ITokenPrincipalMapperSettings
  {
    string anonymous_token_;

    #region ITokenPrincipalMapperSettings Members
    /// <inheritdoc/>
    public string AnonymousToken {
      get { return anonymous_token_; }
      protected set { anonymous_token_ = value; }
    }
    #endregion

    /// <summary>
    /// Creates an instance of the <see cref="ITokenPrincipalMapperSettings"/>
    /// object.
    /// </summary>
    /// <returns></returns>
    void ParseTokenPrincipalMapperSettings() {
      XmlElement local_element =
        GetConfigurationElement(Strings.kTokenPrincipalMapperNode);
      ParseProperties(local_element);
    }
  }
}
