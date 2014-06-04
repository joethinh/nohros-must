using System;
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
    /// Parses the properties that is related with the
    /// <see cref="ITokenPrincipalMapperSettings"/> interface.
    /// </summary>
    void ParseTokenPrincipalMapperSettings() {
      XmlElement local_element =
        GetConfigurationElement(Strings.kTokenPrincipalMapperNode);
      ParseProperties(local_element);
    }
  }
}
