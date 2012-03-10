namespace Nohros.Data.Providers
{
  /// <summary>
  /// Defines method and properties that is used to query relational data
  /// providers.
  /// </summary>
  public interface ISQLDataProvider : IDataProvider
  {
    /// <summary>
    /// Gets the name of the owner of the database.
    /// </summary>
    string DatabaseOwner { get; }
  }
}
