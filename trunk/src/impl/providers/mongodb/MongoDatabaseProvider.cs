using System;
using MongoDB.Driver;

namespace Nohros.Data.MongoDB
{
  public class MongoDatabaseProvider
  {
    readonly string database_;
    readonly MongoServer server_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoDatabaseProvider"/>
    /// class using the specified <paramref name="server"/> and
    /// <paramref name="database"/>.
    /// </summary>
    public MongoDatabaseProvider(MongoServer server, string database) {
      server_ = server;
      database_ = database;
    }
    #endregion

    public MongoDatabase GetDatabase() {
      return server_.GetDatabase(database_);
    }
  }
}
