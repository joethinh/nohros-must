using System;
using System.Collections.Generic;
using System.Xml;
using Nohros.Configuration;

namespace Nohros.Logging.log4net
{
  public class LegacyLoggerFactory : ILoggerFactory
  {
    readonly IConfiguration settings_;

    /// <summary>
    /// Initializes a new instance of the <see cref="LegacyLogger"/> class
    /// using the specified logger settings.
    /// </summary>
    /// <param name="settings">
    /// A <see cref="Nohros.Configuration.IConfiguration"/> object taht can be used to get
    /// configuration inforamtions related with the logger to be created - such
    /// as the configuration of a related logger.
    /// </param>
    protected LegacyLoggerFactory(IConfiguration settings) {
      settings_ = settings;
    }

    /// <summary>
    /// Creates an instance of the <see cref="LegacyLogger"/> class using the
    /// specified provider node.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> object that contains the
    /// options for the logger to be created.
    /// </param>
    /// <returns>
    /// The newly created <see cref="LegacyLogger"/> object.
    /// </returns>
    public ILogger CreateLogger(IDictionary<string, string> options) {
      string logger_name = ProviderOptions.GetIfExists(options,
        Strings.kLoggerName, Strings.kDefaultLoggerName);
      string xml_element_name = ProviderOptions.GetIfExists(options,
        Strings.kLegacyLoggerXmlElementName,
        Strings.kDefaultLegacyLoggerXmlElementName);

      // Get the xml element that is used to configure the legacy log4net
      // logger.
      XmlElement element = settings_.XmlElements[xml_element_name];
      LegacyLogger legacy_logger = new LegacyLogger(element, logger_name);
      legacy_logger.Configure();
      return legacy_logger;
    }
  }
}
