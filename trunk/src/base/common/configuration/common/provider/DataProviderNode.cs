using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// Contains configuration informations for data providers.
  /// </summary>
  public partial class DataProviderNode : ProviderNode, IDataProviderNode
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="DataProviderNode"/> class
    /// using the specified provider name and type.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the provider within the configured
    /// data providers.
    /// </param>
    /// <param name="type">
    /// The assembly-qualified name of the provider's type.
    /// </param>
    /// <remarks>
    /// The provider's location will be set to the application base directory.
    /// </remarks>
    public DataProviderNode(string name, string type)
      : this(name, type, AppDomain.CurrentDomain.BaseDirectory) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataProviderNode"/> class
    /// using the specified provider name and type.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the provider within the configured
    /// data providers.
    /// </param>
    /// <param name="type">
    /// The assembly-qualified name of the provider's type.
    /// </param>
    /// <param name="location">
    /// The path to the directory where the provider assembly is located.
    /// </param>
    public DataProviderNode(string name, string type, string location)
      : base(name, type, location) { }
    #endregion
  }
}
