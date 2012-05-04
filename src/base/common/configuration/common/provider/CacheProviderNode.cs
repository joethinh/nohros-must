using System;

namespace Nohros.Configuration
{
  public partial class CacheProviderNode: ProviderNode, ICacheProviderNode
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheProviderNode"/> class
    /// by using the specified provider name and type.
    /// </summary>
    /// <param name="name">
    /// The name of the provider.
    /// </param>
    /// <param name="type">
    /// The assembly-qualified name of the provider type.
    /// </param>
    protected CacheProviderNode(string name, string type) : base(name, type) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheProviderNode"/> class by
    /// using the specified provider name, type and assembly location.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies a provider within an collection.
    /// </param>
    /// <param name="type">
    /// The provider's assembly-qualified name.
    /// </param>
    /// <param name="location">
    /// The path to the directory where the provider assembly file is stored.
    /// </param>
    protected CacheProviderNode(string name, string type, string location)
      : base(name, type, location) { }
    #endregion
  }
}
