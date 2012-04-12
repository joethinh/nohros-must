using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Nohros.Collections;

namespace Nohros.Configuration
{
  /// <summary>
  /// Represents a single node in the nohros configuration file.
  /// </summary>
  internal interface IConfigurationNode
  {
    /// <summary>
    /// Gets the name of the node.
    /// </summary>
    string Name { get; }
  }
}