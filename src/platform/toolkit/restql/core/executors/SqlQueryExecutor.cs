using System;
using System.Collections.Generic;
using System.Data;
using Nohros.Configuration;
using Nohros.Data;
using Nohros.Data.Json;
using Nohros.Data.Providers;
using Nohros.Caching;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A implementaion of the <see cref="IQueryExecutor"/> interface that
  /// is capable to execute SQL queries.
  /// </summary>
  public partial class SqlQueryExecutor : IQueryExecutor
  {
    readonly ILoadingCache<IConnectionProvider> connection_provider_cache_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlQueryExecutor"/> class
    /// using the specified collection of providers.
    /// </summary>
    /// <param name="connection_provider_cache">
    /// A <see cref="ICache{T}"/> object where we can obtain named instances of
    /// <see cref="IConnectionProvider"/> objects.
    /// </param>
    /// <remarks>
    /// </remarks>
    public SqlQueryExecutor(
      ILoadingCache<IConnectionProvider> connection_provider_cache) {
      if (connection_provider_cache_ == null) {
        throw new ArgumentNullException("connection_provider_cache");
      }
      connection_provider_cache_ = connection_provider_cache;
    }
    #endregion

    #region IQueryExecutor Members
    /// <summary>
    /// Computes the specified query and returns the computation results as a
    /// string.
    /// </summary>
    /// <param name="query">
    /// The query to be processed.
    /// </param>
    /// <remarks>
    /// The format of the returned string is processor dependant. Callers
    /// should check the documentation of each processor to understand the
    /// meaning of the returned string.
    /// </remarks>
    public string Execute(IQuery query) {
      if (query == null) {
        throw new ArgumentNullException("query");
      }

      string provider_name = query.Options[Strings.kConnectionProviderOption];
      IConnectionProvider connection_provider =
        connection_provider_cache_.Get(provider_name);

      using (IDbConnection connection = connection_provider.CreateConnection())
      using (IDbCommand command = GetCommand(connection, query)) {
        connection.Open();
        IJsonCollection data =
          (query.QueryMethod == QueryMethod.Get)
            ? ExecuteReader(command)
            : ExecuteNonQuery(command);
        connection.Close();
        return data.AsJson();
      }
    }

    /// <summary>
    /// Gets a value indicating if a <see cref="IQueryExecutor"/> can execute
    /// the specified query.
    /// </summary>
    /// <param name="query">
    /// The <seealso cref="Query"/> object to check.
    /// </param>
    /// <returns>
    /// <c>true</c> If the executor can execute the query
    /// <paramref name="query"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <seealso cref="Query"/>
    public bool CanExecute(IQuery query) {
      IDictionary<string, string> options = query.Options;
      return
        string.Compare(Strings.kSqlQueryType, query.Type,
          StringComparison.OrdinalIgnoreCase) == 0 &&
            options.ContainsKey(Strings.kConnectionProviderOption);
    }
    #endregion

    IJsonCollection ExecuteReader(IDbCommand command, IJsonCollectionFactory json_collection_factory) {
      IDataReader reader = command.ExecuteReader();
      do {
        while(reader.Read()) {
        }
      }
    }

    IJsonCollection ExecuteNonQuery(IDbCommand command) {
    }

    IDbCommand GetCommand(IDbConnection connection, IQuery query) {
      IDbCommand command = connection.CreateCommand();
      command.CommandText = query.ToString();
      command.CommandType = GetCommandType(query.Options);
      command.CommandTimeout = ProviderOptions.TryGetInteger(query.Options,
        Strings.kCommandTimeoutOption, 30);
      return command;
    }


    CommandType GetCommandType(IDictionary<string, string> options) {
      string command_type_string = ProviderOptions.GetIfExists(
        options, Strings.kCommandTypeOption, Strings.kTextCommandType);
      if (string.Compare(command_type_string, "storedprocedure",
        StringComparison.OrdinalIgnoreCase) == 0) {
        return CommandType.StoredProcedure;
      }
      return CommandType.Text;
    }
  }
}
