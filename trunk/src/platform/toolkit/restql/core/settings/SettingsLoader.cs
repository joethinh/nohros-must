using System;
using Nohros.Configuration;

namespace Nohros.RestQL
{
  public partial class QuerySettings
  {
    public class Loader : AbstractConfigurationLoader<QuerySettings>
    {
      /// <summary>
      /// 
      /// </summary>
      public const string kConfigFileName = Strings.kConfigFileName;

      /// <summary>
      /// 
      /// </summary>
      public const string kConfigRootNode = Strings.kConfigRootNode;

      readonly Builder builder_;

      #region .ctor
      public Loader() : base(new Builder()) {
      }
      #endregion

      /// <inheritdoc/>
      public override QuerySettings CreateConfiguration(
        IConfigurationBuilder<QuerySettings> builder) {
        return builder.Build();
      }
    }
  }
}
