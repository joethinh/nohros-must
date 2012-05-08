using System;
using System.Xml;

namespace Nohros.Configuration
{
  /// <summary>
  /// Provides a default implementation of the <see cref="IXmlElementsNode"/>
  /// interface.
  /// </summary>
  public partial class XmlElementsNode : AbstractHierarchicalConfigurationNode,
                                 IXmlElementsNode
  {
    #region XmlElementNode
    class XmlElementNode : AbstractConfigurationNode
    {
      readonly XmlElement xml_element_;

      #region .ctor
      public XmlElementNode(XmlElement xml_element) : base(xml_element.Name) {
        xml_element_ = xml_element;
      }
      #endregion

      public XmlElement XmlElement {
        get { return xml_element_; }
      }
    }
    #endregion

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlElementsNode"/> class.
    /// </summary>
    public XmlElementsNode() : base(Strings.kXmlElementsNodeName) {
    }
    #endregion

    #region IXmlElementsNode Members
    /// <inheritdoc/>
    public XmlElement GetXmlElement(string xml_element_name) {
      return GetChildNode<XmlElementNode>(xml_element_name).XmlElement;
    }

    /// <inheritdoc/>
    public bool GetXmlElement(string xml_element_name,
      out XmlElement xml_element) {
      XmlElementNode xml_element_node;
      if (GetChildNode(xml_element_name, out xml_element_node)) {
        xml_element = xml_element_node.XmlElement;
        return true;
      }
      xml_element = default(XmlElement);
      return false;
    }

    /// <inheritdoc/>
    public new XmlElement this[string xml_element_name] {
      get { return GetXmlElement(xml_element_name); }
    }
    #endregion
  }
}
