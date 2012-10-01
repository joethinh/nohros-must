using System;
using System.Collections.Generic;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class ProviderOptionsNode
  {
    public static ProviderOptionsNode Parse(string name, XmlElement element,
      out IList<string> references) {
      if (element == null || name == null) {
        throw new ArgumentNullException((element == null)
          ? "element"
          : "name");
      }
      references = new List<string>();
      ProviderOptionsNode options = new ProviderOptionsNode(name);
      foreach (XmlAttribute attribute in element.Attributes) {
        // "ref" attribute indicates a reference to a global provider options.
        // We need to add all the options defined in the global provider to the
        // provider that we are parsing.
        if (Strings.AreEquals(attribute.Name, Strings.kOptionsRefAttribute)) {
          references.Add(attribute.Value);
        } else {
          options[attribute.Name] = attribute.Value;
        }
      }
      return options;
    }
  }
}
