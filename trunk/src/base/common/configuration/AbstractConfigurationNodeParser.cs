using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Nohros.Collections;
using Nohros.Resources;

namespace Nohros.Configuration
{
  public abstract partial class AbstractConfigurationNode
  {
    /// <summary>
    /// Gets the value of an attribute from the given xml node.
    /// </summary>
    /// <param name="node">
    /// The node that contains the attribute to get.
    /// </param>
    /// <param name="name">
    /// The name attribute of the attribute to get.
    /// </param>
    /// <returns>
    /// The value of the attribute named <paramref name="name"/>.
    /// </returns>
    /// <exception cref="ConfigurationException">
    /// A attribute named <paramref name="name"/> is not found within
    /// <paramref name="node"/>.
    /// </exception>
    protected static string GetAttributeValue(XmlElement node, string name) {
      XmlAttribute xml_attribute = node.Attributes[name];
      if (xml_attribute == null) {
        throw new ConfigurationException(
          string.Format(
            StringResources.Configuration_Missing, name, node.Name));
      }
      return xml_attribute.Value;
    }

    /// <summary>
    /// Gets the value of the "location" attribute from the
    /// <paramref name="element"/> and resolves it using the
    /// <paramref name="base_directory"/> as the base directory.
    /// </summary>
    /// <param name="element">
    /// A <see cref="XmlElement"/> that contains an attribute whose name is
    /// "location"
    /// </param>
    /// <param name="base_directory">
    /// The path of the directory to use as the location base directory.
    /// </param>
    /// <returns>
    /// A string representing the absolute path of the value that was set in
    /// the "location" attribute of the <paramref name="element"/>.
    /// </returns>
    protected static string GetLocation(XmlElement element, string base_directory) {
      string location;
      if (GetAttributeValue(element, Strings.kLocationAttribute, out location)) {
        if (!Path.IsPathRooted(location))
          throw new ConfigurationException(
            string.Format(StringResources.Arg_PathRooted, location));
        return Path.Combine(base_directory, location);
      }
      return base_directory;
    }

    /// <summary>
    /// Checks the parser preconditions that is, <paramref name="element"/> and
    /// <paramref name="base_directory"/> are both not <c>null</c> and
    /// <paramref name="base_directory"/> is an absolute path(rooted).
    /// </summary>
    protected static void CheckPreconditions(XmlElement element, string base_directory) {
      if(element == null || base_directory == null) {
        throw new ArgumentNullException(element == null
          ? "element"
          : "base_directory");
      }

      if (!Path.IsPathRooted(base_directory)) {
        throw new ConfigurationException(
          string.Format(StringResources.Arg_PathNotRooted, base_directory));
      }
    }

    /// <summary>
    /// Gets the value of an attribute from the given xml node.
    /// </summary>
    /// <param name="node">
    /// The node that contains the attribute to get.
    /// </param>
    /// <param name="name">
    /// The name attribute of the attribute to get.
    /// </param>
    /// <param name="default_value">
    /// The value that will be returned if an attribute with name
    /// <paramref name="name"/> is not found.
    /// </param>
    /// <returns>
    /// The value of the attribute whose name is <paramref name="name"/> or
    /// <paramref name="default_value"/> if an attribute with
    /// name <paramref name="name"/> is not found.
    /// </returns>
    protected static string GetAttributeValue(XmlElement node, string name,
      string default_value) {
      XmlAttribute xml_attribute = node.Attributes[name];
      return (xml_attribute == null) ? default_value : xml_attribute.Value;
    }

    /// <summary>
    /// Gets the value of an attribute from the given xml node.
    /// </summary>
    /// <param name="element">
    /// The node that contains the attribute.
    /// </param>
    /// <param name="name">
    /// The name of the attribute.
    /// </param>
    /// <param name="value">
    /// When this method returns contains the value of the attribute or
    /// an empty string if the attribute is not found.
    /// </param>
    /// <returns>
    /// <c>true</c> if the atribbute retrieval operation is successful;
    /// otherwise <c>false</c>.
    /// </returns>
    protected static bool GetAttributeValue(XmlElement element, string name,
      out string value) {
      return GetAttributeValue(element, name, string.Empty, out value);
    }

    /// <summary>
    /// Gets the value of an attribute from the given xml node.
    /// </summary>
    /// <param name="element">
    /// The node that contains the attribute.
    /// </param>
    /// <param name="name">
    /// The name of the attribute.
    /// </param>
    /// <param name="value">
    /// When this method returns contains the value of the attribute or
    /// <paramref name="default_value"/> if the attribute is not found.
    /// </param>
    /// <param name="default_value">
    /// The value that will be returned if an attribute with name
    /// <paramref name="name"/> is not found.
    /// </param>
    /// <returns>
    /// <c>true</c> if the atribbute retrieval operation is successful;
    /// otherwise <c>false</c>.
    /// </returns>
    protected static bool GetAttributeValue(XmlElement element, string name,
      string default_value, out string value) {
      XmlAttribute att = element.Attributes[name];
      value = (att != null) ? att.Value : default_value;
      return (att != null);
    }
  }
}
