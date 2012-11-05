using System;
using System.Collections.Generic;
using System.Data;
using Nohros.Data;
using Nohros.Extensions;
using Nohros.Data.Json;
using Nohros.Data.Providers;
using Nohros.Caching;

namespace Nohros.RestQL
{
  /// <summary>
  /// A implementaion of the <see cref="IQueryExecutor"/> interface that
  /// is capable to execute SQL queries.
  /// </summary>
  public abstract class AbstractSqlQueryExecutor : IQueryExecutor
  {
    readonly IJsonCollectionFactory json_collection_factory_;
    readonly RestQLLogger logger_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractSqlQueryExecutor"/> class
    /// using the specified collection of providers.
    /// </summary>
    /// <param name="json_collection_factory">
    /// A <see cref="IJsonCollectionFactory"/> object that can be used to
    /// create instances of the <see cref="IJsonCollection"/> class.
    /// </param>
    protected AbstractSqlQueryExecutor(
      IJsonCollectionFactory json_collection_factory) {
      if (json_collection_factory == null) {
        throw new ArgumentNullException("json_collection_factory");
      }
      json_collection_factory_ = json_collection_factory;
      logger_ = RestQLLogger.ForCurrentProcess;
    }
    #endregion

    protected internal IJsonCollectionFactory JsonCollectionFactory {
      get { return json_collection_factory_; }
    }

    /// <inheritdoc/>
    public virtual string Execute(IQuery query,
      IDictionary<string, string> parameters) {
      if (query == null) {
        throw new ArgumentNullException("query");
      }

      IConnectionProvider provider;
      if (GetConnectionProvider(query.Options, out provider)) {
        return Execute(query, parameters, provider);
      }

      logger_.Warn(string.Format(
        Resources.SqlQueryExecutor_ProviderNotFound_Query, query.Name));
      return string.Empty;
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

    public abstract bool GetConnectionProvider(
      IDictionary<string, string> options, out IConnectionProvider provider);

    protected virtual string Execute(IQuery query,
      IDictionary<string, string> parameters, IConnectionProvider provider) {
      using (IDbConnection connection = provider.CreateConnection())
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
            string.Format(Resources.QueryExecutor_Missing_ParameterValue, name));
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
