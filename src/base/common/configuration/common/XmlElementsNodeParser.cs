using System;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class XmlElementsNode
  {
    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="XmlElementsNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the xml elements configuration data.
    /// </param>
    /// <returns>
    /// A <see cref="XmlElementsNode"/> containing the configured xml elements.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a <c>null</c> reference.
    /// </exception>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    public static XmlElementsNode Parse(XmlElement element) {
      if (element == null) {
        throw new ArgumentNullException("element");
      }

      XmlElementsNode xml_elements_node = new XmlElementsNode();
      foreach (XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element) {
          xml_elements_node.AddChildNode(new XmlElementNode(element));
        }
      }
      return xml_elements_node;
    }
  }
}
