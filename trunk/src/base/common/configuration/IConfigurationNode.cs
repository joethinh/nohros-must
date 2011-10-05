using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Nohros.Data.Collections;

namespace Nohros.Configuration
{
  /// <summary>
  /// Represents a single node in the nohros configuration file.
  /// </summary>
  internal interface IConfigurationNode
  {
    /// <summary>
    /// Parses the content of the XML node.
    /// </summary>
    /// <param name="node">A XML node containing hte data to parse.</param>
    /// <param name="config">The configuration object which this node belongs
    /// to.</param>
    //void Parse(XmlNode node, DictionaryValue nodes);

    /// <summary>
    /// Gets the name of the node.
    /// </summary>
    string Name { get; }
  }
}