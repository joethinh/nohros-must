using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// The application settings.
  /// </summary>
  public class Settings : NohrosConfiguration, ISettings
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Settings"/> class.
    /// </summary>
    public Settings() {
    }
    #endregion

    protected override void OnLoadComplete(System.EventArgs e) {
      base.OnLoadComplete(e);
    }
  }
}
