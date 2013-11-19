using System;
using System.Xml;

namespace Nohros.Configuration
{
  internal partial class ReplicasNode
  {
    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="ReplicasNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the providers configuration data.
    /// </param>
    /// <returns>
    /// A <see cref="ReplicasNode"/> containing the configured providers.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a <c>null</c> reference.
    /// </exception>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    public static ReplicasNode Parse(XmlElement element) {
      string group = GetAttributeValue(element, Strings.kGroupAttribute);
      ReplicasNode replicas = new ReplicasNode(group);
      foreach (XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element &&
          Strings.AreEquals(node.Name, Strings.kReplicaNodeName)) {
          ReplicaNode replica = ReplicaNode.Parse((XmlElement) node);
          replicas.AddChildNode(replica);
        }
      }
      return replicas;
    }
  }
}
