using System;
using System.Collections.Generic;
using System.Data;
using Nohros.Data;
using Nohros.Extensions;
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
    readonly IJsonCollectionFactory json_collection_factory_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlQueryExecutor"/> class
    /// using the specified collection of providers.
    /// </summary>
    /// <param name="connection_providers_cache">
    /// A <see cref="ILoadingCache{T}"/> object where we can obtain named
    /// instances of <see cref="IConnectionProvider"/> objects.
    /// </param>
    /// <param name="json_collection_factory">
    /// A <see cref="IJsonCollectionFactory"/> object that can be used to
    /// create instances of the <see cref="IJsonCollection"/> class.
    /// </param>
    /// <remarks>
    /// </remarks>
    public SqlQueryExecutor(
      ILoadingCache<IConnectionProvider> connection_providers_cache,
      IJsonCollectionFactory json_collection_factory) {
      if (connection_providers_cache == null || json_collection_factory == null) {
        throw new ArgumentNullException(connection_providers_cache == null
          ? "connection_providers_cache"
          : "json_collection_factory");
      }
      connection_provider_cache_ = connection_providers_cache;
      json_collection_factory_ = json_collection_factory;
    }
    #endregion

    /// <inheritdoc/>
    public string Execute(IQuery query, IDictionary<string, string> parameters) {
      if (query == null) {
        throw new ArgumentNullException("query");
      }

      string provider_name = query.Options[Strings.kConnectionProviderOption];
      IConnectionProvider connection_provider =
        connection_provider_cache_.Get(provider_name);

      using (IDbConnection connection = connection_provider.CreateConnection())
      using (var builder = new CommandBuilder(connection)) {
        builder
          .SetText(query.QueryText)
          .SetType(GetCommandType(query.Options))
          .SetTimeout(query.Options
            .GetInteger(Strings.kCommandTimeoutOption, 30));

        BindParameters(builder, query.Parameters, parameters);

        IDbCommand cmd = builder.Build();
        connection.Open();
        string response =
          (query.QueryMethod == QueryMethod.Get)
            ? ExecuteReader(cmd, query)
            : ExecuteNonQuery(cmd, query);
        connection.Close();
        return response;
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

    string ExecuteReader(IDbCommand command, IQuery query) {
      IDataReader reader = command.ExecuteReader();
      string preferred_json_collection = query.Options
        .GetString(Strings.kJsonCollectionOption,
          Strings.kDefaultJsonCollection);
      IJsonCollection json_collection =
        json_collection_factory_
          .CreateJsonCollection(preferred_json_collection, reader);
      return Serialize(json_collection, json_collection.Count);
    }

    string ExecuteNonQuery(IDbCommand command, IQuery query) {
      int no_of_affected_records = command.ExecuteNonQuery();
      string preferred_json_collection = query.Options
        .GetString(Strings.kJsonCollectionOption,
          Strings.kDefaultJsonCollection);
      IJsonCollection json_collection =
        json_collection_factory_
          .CreateJsonCollection(preferred_json_collection);
      return Serialize(json_collection, no_of_affected_records);
    }

    string Serialize(IJsonCollection json_collection, int no_of_affected_rows) {
      JsonStringBuilder builder = new JsonStringBuilder()
        .WriteBeginObject()
        .WriteMember(Strings.kResponseAffectedRowsMemberName,
          no_of_affected_rows)
        .WriteMemberName(Strings.kResponseDataMemberName)
        .WriteUnquotedString(json_collection.AsJson())
        .WriteEndObject();
      return builder.ToString();
    }

    void BindParameters(CommandBuilder builder, IEnumerable<string> names,
      IDictionary<string, string> values) {
      foreach (string name in names) {
        string value;
        if (!values.TryGetValue(name, out value)) {
          throw new KeyNotFoundException(
            string.Format(Resources.QueryExecutor_Missing_ParameterValue));
        }
        builder.AddParameterWithValue(name, value);
      }
    }


    CommandType GetCommandType(IDictionary<string, string> options) {
      string command_type_string = options
        .GetString(Strings.kCommandTypeOption, Strings.kTextCommandType);
      if (command_type_string.CompareOrdinalIgnoreCase(
        Strings.kStoredProcedureCommandType)) {
        return CommandType.StoredProcedure;
      }
      return CommandType.Text;
    }
  }
}
