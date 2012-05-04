using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Configuration
{
  /// <summary>
  /// Defines all the constant string that could appear on the Xml
  /// configuration files.
  /// </summary>
  internal sealed class Strings
  {
    internal const string kBaseDirectoryAttribute = "base-directory";
    internal const string kNameAttribute = "name";
    internal const string kLocationAttribute = "location";
    internal const string kTypeAttribute = "type";
    internal const string kAliasAttribute = "alias";

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
  }
}