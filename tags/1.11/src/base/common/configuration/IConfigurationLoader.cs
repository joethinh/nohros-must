using System;
using System.Xml;
using System.IO;

namespace Nohros.Configuration
{
  /// <summary>
  /// A class used to build a <see cref="IConfiguration"/> object from a XML
  /// file.
  /// </summary>
  public interface IConfigurationLoader<T> where T : IConfiguration
  {
    /// <summary>
    /// Loads the configuration values based on the application's configuration
    /// settings and watch it for modifications.
    /// </summary>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as
    /// the application whith ' .config ' appended.
    /// <para>This file is XML and calling this function prompts the loader to
    /// look in that file for a key named [NohrosConfigurationFile] that
    /// contains the path for the configuration file.
    /// </para>
    /// <para>
    /// The value of the [NohrosConfigurationFile] must be absolute or relative
    /// to the application base directory.
    /// </para>
    /// <para>
    /// The xml document must constains at leats one node with name euqlas to
    /// "nohros" that is descendant of the root node.
    /// </para>
    /// </remarks>
    /// <para>
    /// This methods watches the nohros configuration file for modifications
    /// and when it is modified the configuration values is reloaded.
    /// </para>
    T LoadAndWatch();

    /// <summary>
    /// Loads the configuration values based on the application's configuration
    /// settings and watch it for modifications.
    /// </summary>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as
    /// the application whith ' .config ' appended. This file is XML and
    /// calling this function prompts the loader to look in that file for a key
    /// named [NohrosConfigurationFile] that contains the path for the
    /// configuration file.
    /// <para>
    /// The value of the [NohrosConfigurationFile] must be absolute or relative
    /// to the application base directory.
    /// </para>
    /// <para>
    /// The configuration file must be valid XML. It must contain at least one
    /// element called <paramref name="root_node_name"/> that contains the
    /// configuration data. Note that a element with name "nohros" must
    /// exists some place on the root nodes tree.
    /// </para>
    /// <para>
    /// This methods watches the nohros configuration file for modifications
    /// and when it is modified the configuration values is reloaded.
    /// </para>
    /// </remarks>
    T LoadAndWatch(string root_node_name);

    /// <summary>
    /// Loads the configuration values based on the application's configuration
    /// settings.
    /// </summary>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as
    /// the application whith ' .config ' appended.
    /// <para>This file is XML and calling this function prompts the loader to
    /// look in that file for a key named [NohrosConfigurationFile] that
    /// contains the path for the configuration file.
    /// </para>
    /// <para>
    /// The value of the [NohrosConfigurationFile] must be absolute or relative
    /// to the application base directory.
    /// </para>
    /// <para>
    /// The xml document must constains at leats one node with name euqlas to
    /// "nohros" that is descendant of the root node.
    /// </para>
    /// </remarks>
    T Load();

    /// <summary>
    /// Loads the configuration values based on the application's configuration
    /// settings.
    /// </summary>
    /// <remarks>
    /// Each application has a configuration file. This has the same name as
    /// the application whith ' .config ' appended. This file is XML and
    /// calling this function prompts the loader to look in that file for a key
    /// named [NohrosConfigurationFile] that contains the path for the
    /// configuration file.
    /// <para>
    /// The value of the [NohrosConfigurationFile] must be absolute or relative
    /// to the application base directory.
    /// </para>
    /// <para>
    /// The configuration file must be valid XML. It must contain at least one
    /// element called <paramref name="root_node_name"/> that contains the
    /// configuration data. Note that a element with name "nohros" must
    /// exists some place on the root nodes tree.
    /// </para>
    /// </remarks>
    T Load(string root_node_name);

    T Load(string config_file_name, string root_node_name);

    T Load(FileInfo config_file_info, string root_node_name);

    T Load(XmlElement element);

    T LoadAndWatch(string config_file_name, string root_node_name);

    T LoadAndWatch(FileInfo config_file_info, string root_node_name);

    /// <summary>
    /// Gets the directory path where the configuration file is stored.
    /// </summary>
    /// <returns>
    /// An string that represents the location of the configuration file or the
    /// application base directory if the location could not be retrieved.
    /// </returns>
    string Location { get; }

    /// <summary>
    /// Gets the date and time when one of the Load methods was last called.
    /// </summary>
    DateTime Version { get; }
  }
}
