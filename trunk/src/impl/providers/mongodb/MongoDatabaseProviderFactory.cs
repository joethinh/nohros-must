using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Driver;
using Nohros.Extensions;

namespace Nohros.Data.MongoDB
{
  public class MongoDatabaseProviderFactory
  {
    /// <summary>
    /// The key that shoul be associated with the option that contains
    /// the mongodb connection string.
    /// </summary>
    /// <remarks>
    /// This option is mutually exclusive with the others (except
    /// <see cref="kDatabaseOption"/>, which is mandatory) and has the hightest
    /// priority.
    /// </remarks>
    public const string kConnectionStringOption = "connectionString";

    /// <summary>
    /// Identifies a server address to connects to. It identifies either a
    /// hostname, ip address, or UNIX domain socket.
    /// </summary>
    /// <remarks>
    /// If the server is not running on the default port it should be specified
    /// through this key using the following format [host]:[port].
    /// <para>
    /// You can specify as many hosts as necessary. You would specify multiple
    /// hosts, for example to replica sets.
    /// </para>
    /// <para>
    /// You should specify at least one host if the
    /// <seealso cref="kConnectionStringOption"/> is not defined.
    /// </para>
    /// </remarks>
    public const string kHostOption = "host";

    /// <summary>
    /// The name of the user that should be used to connect to the server.
    /// </summary>
    /// <remarks>
    /// This parameter is optional and if specified it will be used to log
    /// in to the specified database using this username after connecting to
    /// the mongodb instance.
    /// <para>
    /// If this option is present the <see cref="kPasswordOption"/> should
    /// be defined.
    /// </para>
    /// </remarks>
    public const string kUserNameOption = "username";

    /// <summary>
    /// The password of the user that should be used to connect to the server
    /// </summary>
    public const string kPasswordOption = "password";

    /// <summary>
    /// The name of the database.
    /// </summary>
    /// <remarks>
    /// This option is required.
    /// </remarks>
    public const string kDatabaseOption = "database";

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public MongoDatabaseProvider CreateProvider(
      IDictionary<string, string> options) {
      string connection_string;
      string database = options.GetString(kDatabaseOption);
      if (!options.TryGetValue(kConnectionStringOption, out connection_string)) {
        var builder = new MongoConnectionStringBuilder();
        string user_name, password;
        options.TryGetValue(kUserNameOption, out user_name);
        options.TryGetValue(kPasswordOption, out password);

        user_name = options.GetString(kUserNameOption, null);
        if (user_name != null) {
          builder.Username = user_name;
          builder.Password = options.GetString(kPasswordOption);
          builder.DatabaseName = database;
        }

        var servers = new List<MongoServerAddress>();
        foreach (KeyValuePair<string, string> pair in options) {
          if (pair.Value.CompareOrdinal(kHostOption)) {
            string host = pair.Value;
            int index = host.IndexOf(":");
            if (index == -1) {
              servers.Add(new MongoServerAddress(host));
            }
            servers.Add(new MongoServerAddress(host.Substring(0, index)));
          }
        }

        if (servers.Count == 0) {
          throw new KeyNotFoundException(
            "At least one host should be specified.");
        }
        builder.Servers = servers;
        connection_string = builder.ToString();
      }
      var client = new MongoClient(connection_string);
      var server = client.GetServer();
      return new MongoDatabaseProvider(server, database);
    }
  }
}
