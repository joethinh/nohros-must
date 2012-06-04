using System;
using System.Xml;

namespace Nohros.Toolkit.RestQL
{
  public class MongooseSettings : Settings
  {
    protected override void OnLoadComplete() {
      base.OnLoadComplete();
      ParseMongooseSettings();
    }

    public static void MongooseSettings CreateSettings() {
    }

    void ParseMongooseSettings() {
      XmlElement local_element =
        GetConfigurationElement(Strings.kMongooseXmlNode);
      ParseProperties(local_element);
    }
  }
}
