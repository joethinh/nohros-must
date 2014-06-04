using System;
using System.Collections.Generic;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class ProviderOptionsNode
  {
    public static ProviderOptionsNode Parse(string name, XmlElement element,
      out IList<string> references) {
      if (element == null) {
        throw new ArgumentNullException("element");
      }

      references = new List<string>();
      ProviderOptionsNode options = new ProviderOptionsNode(name);
      foreach (XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element &&
          Strings.AreEquals(node.Name, Strings.kOptionNodeName)) {
          string reference;
          if (GetAttributeValue((XmlElement) node, Strings.kOptionsRefAttribute,
            out reference)) {
            // A node named "ref" indicates a reference to a global provider
            // options. We need to add all the options defined in the global
            // provider to the provider that we are parsing.
            references.Add(reference);
          } else {
            string option_name = GetAttributeValue((XmlElement) node,
              Strings.kNameAttribute);
            string opttion_value = GetAttributeValue((XmlElement)node,
              Strings.kValueAttribute, string.Empty);
            options[option_name] = opttion_value;
          }
        }
      }
      return options;
    }
  }
}
