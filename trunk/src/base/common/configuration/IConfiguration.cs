using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Nohros.Configuration
{
  /// <summary>
  /// Provides an interface to access and manipulate configuration files.
  /// </summary>
  public interface IConfiguration
  {
    /// <summary>
    /// Loads the configuration values by parsing the application's
    /// configuration settings.
    /// </summary>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as
    /// the application whith ' .config ' appended. This file format is Xml and
    /// calling this function prompts the loader to look in that file for a
    /// section called <c>appconfig</c> that contains the configuration data.
    /// </remarks>
    /// <exception cref="ConfigurationException">
    /// A element with name "appconfig" could not be found into the application
    /// configuration file.
    /// </exception>
    /// <seealso cref="Load(string)"/>
    /// <seealso cref="Load(XmlElement)"/>
    void Load();

    /// <summary>
    /// Loads the configuration by parsing the application's configuration
    /// settings.
    /// </summary>
    /// <param name="root_node_name">
    /// The xpath of the node that contains the configuration data.
    /// </param>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as
    /// the application whith ' .config ' appended. This file is format is Xml
    /// and calling this function prompts the loader to look in that file for
    /// a section called <paramref name="root_node_name"/> that contains the
    /// configuration data.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// A element whose xpath is <paramref name="root_node_name"/> could not
    /// be found into the loaded configuration file.
    /// </exception>
    /// <seealso cref="Load(XmlElement)"/>
    void Load(string root_node_name);

    /// <summary>
    /// Load the configuration by parsing the specified configuration file.
    /// </summary>
    /// <param name="config_file_name">
    /// The name of the configuration file.
    /// </param>
    /// <param name="root_node_name">
    /// The xpath of the node that contains the configuration data.
    /// </param>
    /// <remarks>
    /// If the <paramref name="root_node_name"/> is null, the first Xml element
    /// will be used as root node.
    /// <para>
    /// This method assumes that the specified configuration file is located
    /// in the application base directory.
    /// </para>
    /// <para>
    /// The configuration file must be valid Xml. It must contain at least one
    /// element called <paramref name="root_node_name"/>.
    /// </para>
    /// <para>
    /// For backward compatibility this method allows the
    /// <see cref="root_node_name"/> to be a null reference and when is the
    /// case the first valid Xml element will be used like the configuration
    /// root node.
    /// </para>
    /// <para>
    /// If you need to monitor this file for changes and reload the
    /// configuration when the config file's contents changes then you should
    /// use the <see cref="LoadAndWatch(string, string)"/> method instead.
    /// </para>
    /// </remarks>
    /// <exception cref="FileNotFoundException">
    /// The configuration file does not exists or is not located into the
    /// application base directory.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// A element with name <paramref name="root_node_name"/> could not be
    /// found into the loaded configuration file.
    /// </exception>
    /// <seealso cref="Load(string, string)"/>
    /// <seealso cref="Load(FileInfo, string)"/>
    void Load(string config_file_name, string root_node_name);

    /// <summary>
    /// Load the configuation values using the specified configuration file and
    /// node name.
    /// </summary>
    /// <param name="config_file_info">
    /// The Xml configuration file to load the configuration from.
    /// </param>
    /// <param name="root_node_name">
    /// The xpath of the node that contains the configuration data.
    /// </param>
    /// <exception cref="FileNotFoundException">
    /// If the config file does not exists.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="config_file_info"/> is a null reference.</exception>
    /// <exception cref="ArgumentException">
    /// A element with name <paramref name="root_node_name"/> could not be
    /// found into the configuration file.
    /// </exception>
    /// <remarks>
    /// <para>
    /// The config file could be specified into the main application
    /// configuration file through a custom configuration key. To load the
    /// configuration use code like above:
    /// </para>
    /// <para>
    /// For backward compatibility this method allows the
    /// <paramref name="root_node_name"/> to be a
    /// <c>null</c> reference and when is the case the first Xml element will
    /// be used like the configuration root node.
    /// </para>
    /// <code>
    /// using Nohros.Configuration;
    /// using System.IO;
    /// using System.Configuration;
    /// 
    /// ...
    /// string app_base_directory =  AppDomain.CurrentDomain.BaseDirectory;
    /// string config_file_name =
    ///   ConfigurationSettings.AppSettings["my-custom-config-file-path"];
    /// string config_file_path =
    ///   Path.Combine(app_base_directory, config_file_name);
    /// 
    /// FileInfo config_file = new FileInfo(config_file_path);
    /// AbstractConfiguration config_object = MyFactory.GetConfigObject();
    /// config_object.Load(config_file, "my-config-root-node");
    /// </code>
    /// <para>
    /// In your application configuration file you must specify the
    /// configuration file to use like this:
    /// </para>
    /// <code>
    /// &lt;configuration&gt;
    ///		&lt;appSettings&gt;
    ///			&lt;add key="my-custom-config-file-path" value="MyCustom.config"/&gt;
    ///		&lt;/appSettings&gt;
    ///	&lt;/configuration&gt;
    /// </code>
    /// In that case your configuration file must have a node named
    /// "my-config-root-node".
    /// <para>
    /// If you need to monitor this file for changes and reload the
    /// configuration when the config file's contents changes then you should
    /// use the <see cref="LoadAndWatch(FileInfo, string)"/> method instead.
    /// </para>
    /// </remarks>
    void Load(FileInfo config_file_info, string root_node_name);

    /// <summary>
    /// Load the configuration values parsing the specified XML element.
    /// </summary>
    /// <remarks>
    /// Load the configuration from the given <paramref name="element"/>
    /// element.
    /// </remarks>
    /// <param name="element">
    /// The <see cref="XmlElement"/> containing the configuration values to
    /// loadn and parse.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a null reference.
    /// </exception>
    void Load(XmlElement element);

    /// <summary>
    /// Load the configuration values using the specified configuration file,
    /// monitor the file for changes and reload the configuration if a change
    /// is detected.
    /// </summary>
    /// <param name="config_file_name">
    /// The name of the configuration file.
    /// </param>
    /// <param name="root_node_name">The xpath of the node that contains the
    /// configuration data.
    /// </param>
    /// <remarks>
    /// If the <paramref name="root_node_name"/> is null, the first Xml element
    /// will be used as a root node.
    /// <para>
    /// This method assumes that the specified configuration file is located
    /// in the application base directory.
    /// </para>
    /// <para>
    /// The configuration file must be valid Xml. It must contain at least one
    /// element called <paramref name="root_node_name"/> that contains the
    /// configuration data.
    /// </para>
    /// </remarks>
    /// <exception cref="FileNotFoundException">
    /// The configuration file does not exists or is not located in the
    /// application base directory.
    /// </exception>
    void LoadAndWatch(string config_file_name, string root_node_name);

    /// <summary>
    /// Load the configuration values using the file specified, monitor the
    /// file for changes and reload the configuration if a change is detected.
    /// </summary>
    /// <param name="config_file_info">
    /// The xml file to load the configuration from.
    /// </param>
    /// <param name="root_node_name">
    /// The xpath of the node that contains the configuration data.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="config_file_info"/> is null.
    /// </exception>
    /// <remarks>
    /// If the <paramref name="root_node_name"/> is null, the first xml
    /// element will be used as a root node.
    /// <para>
    /// The configuration file must be valid XML. It must contain at least one
    /// element called <paramref name="root_node_name"/> that contains the
    /// configuration data.
    /// </para>
    /// </remarks>
    void LoadAndWatch(FileInfo config_file_info, string root_node_name);

    /// <summary>
    /// Gets the directory path where the configuration file is stored.
    /// </summary>
    /// <returns>
    /// An string that represents the location of the configuration file or
    /// the application base directory if the location could not be retrieved.
    /// </returns>
    string Location { get; }
  }
}
