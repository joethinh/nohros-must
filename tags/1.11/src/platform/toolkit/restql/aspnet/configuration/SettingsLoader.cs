using System;
using Nohros.Configuration;

namespace Nohros.RestQL
{
  public partial class Settings : IConfiguration
  {
    public class Loader : AbstractConfigurationLoader<Settings>
    {
      #region .ctor
      public Loader() : base(new Builder()) {
      }
      #endregion
    }
  }
}
