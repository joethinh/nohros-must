using System;
using System.Xml;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// <see cref="WebSettings"/> objtect contains the application
  /// configuration data.
  /// </summary>
  public class WebSettings : Settings
  {
    #region .ctor
    public WebSettings() {
    }
    #endregion

    protected override void OnLoadComplete() {
      base.OnLoadComplete();
      ParseZmqSettings();
    }

    void ParseZmqSettings() {
      XmlElement local_element =
        GetConfigurationElement(Strings.kAspNetXmlNode);
      ParseProperties(local_element);
    }
  }
}
