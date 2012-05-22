using System;

using Nohros.Configuration;

namespace Nohros
{
  /// <summary>
  /// Defines the constant string that could appear on the nohros
  /// configuration files.
  /// </summary>
  public sealed class Strings
  {
    internal const string kNohrosNodeName = "nohros";

    internal const string kBaseDirectoryAttribute = "base-directory";
    internal const string kNameAttribute = "name";
    internal const string kLocationAttribute = "location";
    internal const string kTypeAttribute = "type";
    internal const string kGroupAttribute = "group";

    internal const string kControlFlagAttribute = "control-flag";

    // node names
    internal const string kCommonNodeName = "common";
    internal const string kWebNodeName = "common";
    internal const string kRepositoriesNodeName = "repositories";
    internal const string kRepositoryNodeName = "repository";
    internal const string kProvidersNodeName = "providers";
    internal const string kDataProvidersNodeName = "data";
    internal const string kCacheProvidersNodeName = "cache";
    internal const string kSimpleProvidersNodeName = "simple";
    internal const string kProviderNodeName = "provider";
    internal const string kOptionsNodeName = "options";
    internal const string kLoginModulesNodeName = "login-modules";
    internal const string kLoginModuleNodeName = "module";
    internal const string kXmlElementsNodeName = "xml-elements";

    /// <summary>
    /// The key that will be used to store the Xml element that was used to
    /// configure a <see cref="IMustConfiguration"/> object.
    /// </summary>
    public const string kRootXmlElementName = kNohrosNodeName;
  }
}