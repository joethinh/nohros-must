using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="IXmlElementsNode"/> is a container for xml elements. This is
  /// typically used to configure third party libraries that con be configured
  /// using a Xml file - such as log4net loggin library.
  /// </summary>
  public interface IXmlElementsNode: IConfigurationNode
  {
    /// <summary>
    /// Gets a <see cref="XmlElement"/> node whose name is
    /// <paramref name="xml_element_name"/>.
    /// </summary>
    /// <param name="xml_element_name">
    /// The name of the xml element.
    /// </param>
    /// <returns>
    /// A <see cref="XmlElement"/> object whose name is
    /// <paramref name="xml_element_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A <see cref="XmlElement"/> object whose name is
    /// <param name="xml_element_name"> was not found.
    /// </param>
    /// </exception>
    XmlElement GetXmlElement(string xml_element_name);

    /// <summary>
    /// Gets a <see cref="XmlElement"/> node whose name is
    /// <paramref name="xml_element_name"/>.
    /// </summary>
    /// <param name="xml_element_name">
    /// The name of the provider.
    /// </param>
    /// <param name="xml_element">
    /// When this method returns contains a <see cref="XmlElement"/>
    /// object whose name is <paramref name="xml_element_name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a provider whose name is
    /// <paramref name="xml_element_name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    bool GetXmlElement(string xml_element_name, out XmlElement xml_element);

    /// <summary>
    /// Gets a <see cref="XmlElement"/> node whose name is
    /// <paramref name="xml_element_name"/>.
    /// </summary>
    /// <param name="xml_element_name">
    /// The name of the xml element.
    /// </param>
    /// <returns>
    /// A <see cref="XmlElement"/> object whose name is
    /// <paramref name="xml_element_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A <see cref="XmlElement"/> object whose name is
    /// <param name="xml_element_name"> was not found.
    /// </param>
    /// </exception>
    /// <remarks>
    /// This method is a shortcut for the <see cref="GetXmlElement(string)"/>
    /// method.
    /// </remarks>
    IProviderNode this[string xml_element_name] { get; }
  }
}
