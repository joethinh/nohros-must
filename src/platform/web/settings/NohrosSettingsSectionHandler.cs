using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Web;
using System.IO;

namespace Nohros.Net
{
    internal class NohrosSettingsSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates the configuration section handler
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            NetSettings settings = new NetSettings();

			HttpContext ctx = HttpContext.Current;

			XmlAttributeCollection attributes = section.Attributes;

			bool b;
			XmlAttribute att = attributes["isPathInUrl"];
			if (att != null)
				if (bool.TryParse(att.Value, out b))
					settings.IsPathInUrl = b;

			if (!settings.IsPathInUrl)
			{
				att = attributes["stylePath"];
				if (att != null)
					settings.StylePath = att.Value;

				att = attributes["scriptPath"];
				if (att != null)
					settings.ScriptPath = att.Value;
			}

			foreach (XmlNode child in section)
			{
				if (child.Name == "scriptReplacements")
					GetScriptReplacements(child, settings);

                if (child.Name == "plugins")
                    GetPlugins(child, settings);
			}
            return settings;
        }

		/// <summary>
		/// Replaces the default scripts
		/// </summary>
		/// <param name="node">XmlNode for the web.config XML</param>
		/// <param name="settings">Object that hold the config values</param>
		private void GetScriptReplacements(XmlNode node, NetSettings settings)
		{
            string name;
            XmlAttribute attribute;

		    foreach (XmlNode script in node.ChildNodes)
		    {
                if (script.NodeType == XmlNodeType.Element)
                {
			        switch (script.Name)
			        {
				        case "file":
                            attribute = script.Attributes["name"];
                            if (attribute != null)
                            {
                                name = attribute.Value;
                                attribute = script.Attributes["path"];
                                if (name.Length > 0 && attribute != null)
                                    settings.Scripts[name] = attribute.Value;
                            }
					        break;
			        }
                }
		    }
		}

        /// <summary>
        /// Gets the plugins that must be merged into the core
        /// </summary>
        /// <param name="node">XmlNode for the web.config XML</param>
        /// <param name="settings">Object that hold the config values</param>
        private void GetPlugins(XmlNode node, NetSettings settings)
        {
            // Get the path for the plugins directory.
            XmlAttribute att = node.Attributes["path"];
            string path = null;
            if (att != null) {
                path = Utility.MapPath(att.Value);
                if(Directory.Exists(path))
                    settings.PluginsPath = path;
            }

            // If the path is invalid or is not supplied we try to use
            // the plugins folder inside the application BaseDirectory.
            if (settings.PluginsPath == null) {
                path = Utility.BaseDirectory + "plugins";
                if(Directory.Exists(path))
                    settings.PluginsPath = path; 
            }

            // we cannot found a valid plugins folder, the plugins will not
            // be loaded
            if (settings.PluginsPath == null) return;

            // add/remove plugins
            foreach(XmlNode plugin in node.ChildNodes)
            {
                if(plugin.NodeType == XmlNodeType.Element)
                {
                    XmlAttribute attribute = plugin.Attributes["name"];

                    if(attribute == null)
                        continue;

                    switch (plugin.Name)
                    {
                        case "add":
                            attribute = plugin.Attributes["file"];
                            settings.Plugins.Add( (attribute == null) ? string.Empty : attribute.Value );
                            break;

                        case "remove":
                            settings.Plugins.Remove(attribute.Value);
                            break;
                    }
                }
            }
        }
    }
}
