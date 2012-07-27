using System;
using System.Xml;
using log4net;
using log4net.Config;

namespace Nohros.Logging.log4net
{
  public class LegacyLogger : AbstractLogger
  {
    readonly XmlElement element_;
    readonly string logger_name_;

    /// <summary>
    /// Initializes a new instance of the <see cref="LegacyLogger"/> class
    /// using the specified xml elelement.
    /// </summary>
    /// <param name="element">
    /// A <see cref="XmlElement"/> that represents a log4net xml configuration
    /// element.
    /// </param>
    /// <param name="logger_name">
    /// The name of the logger related with the given <paramref name="element"/>.
    /// </param>
    /// <remarks>
    /// The <paramref name="element"/> will be used to configure the log4net
    /// library and the <paramref name="logger_name"/> will be used to get a
    /// named logger throught the log4net <see cref="LogManager"/> class.
    /// </remarks>
    public LegacyLogger(XmlElement element, string logger_name) {
      element_ = element;
      logger_name_ = logger_name;
    }

    /// <summary>
    /// Configures the <see cref="LegacyLogger"/> logger.
    /// </summary>
    public void Configure() {
      XmlConfigurator.Configure(element_);
      logger = LogManager.GetLogger(logger_name_);
    }
  }
}
