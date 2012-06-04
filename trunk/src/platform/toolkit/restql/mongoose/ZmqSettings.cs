using System;
using System.Xml;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// <see cref="ZmqSettings"/> objtect contains the application
  /// configuration data.
  /// </summary>
  public class ZmqSettings : Settings, IZmqSettings
  {
    string ports_;

    public ZmqSettings() {
      ports_ = "8800";
    }

    protected override void OnLoadComplete() {
      base.OnLoadComplete();
      ParseZmqSettings();
    }

    /// <inheritdoc/>
    public string Ports {
      get { return ports_; }
      protected set { ports_ = value; } // dynamically assigned by base class.
  }

    void ParseZmqSettings() {
      XmlElement local_element =
        GetConfigurationElement(Strings.kZmqXmlNode);
      ParseProperties(local_element);
    }
  }
}
