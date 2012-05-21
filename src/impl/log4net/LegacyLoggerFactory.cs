using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Nohros.Configuration;

namespace Nohros.Logging.log4net
{
  public partial class LegacyLogger : ILoggerFactory
  {
    #region ILoggerFactory Members
    /// <summary>
    /// Creates an instance of the <see cref="LegacyLogger"/> class using the
    /// specified provider node.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> object that contains the
    /// options for the logger to be created.
    /// </param>
    /// <param name="configuration">
    /// A <see cref="IMustConfiguration"/> object taht can be used to get
    /// configuration inforamtions related with the logger to be created - such
    /// as the configuration of a related logger.
    /// </param>
    /// <returns>
    /// The newly created <see cref="LegacyLogger"/> object.
    /// </returns>
    public ILogger CreateLogger(IDictionary<string, string> options,
      IMustConfiguration configuration) {
      string logger_name = options[Strings.kLoggerName];
      string xml_element_name = ProviderOptions.GetIfExists(options,
        Strings.kLegacyLoggerXmlElementName,
        Strings.kDefaultLegacyLoggerXmlElementName);

      // Get the xml element that is used to configure the legacy log4net
      // logger.
      XmlElement element =
        configuration.XmlElements[xml_element_name];
      LegacyLogger legacy_logger = new LegacyLogger(element, logger_name);
      legacy_logger.Configure();
      return legacy_logger;
    }
    #endregion
  }
}
