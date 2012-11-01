using System;
using System.Data;
using System.Data.SqlClient;
using Nohros.Configuration;
using Nohros.Data;
using Nohros.Data.Providers;
using Nohros.Extensions;
using Nohros.Resources;

namespace Nohros.RestQL
{
  public class SqlQueryDataProvider : IQueryDataProvider
  {
    const string kClassName = "Nohros.RestQL.SqlQueryDataProvider";

    const string kGetConnectionProviders =
      ".rql_query_get_connection_providers";

    const string kGetQuery = ".rql_query_get";

    readonly RestQLLogger logger_;
    readonly SqlConnectionProvider sql_connection_provider_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="IConnectionProvider"/>
    /// using the specified <see cref="IConnectionProvider"/> connection
    /// provider.
    /// </summary>
    /// <param name="sql_connection_provider">
    /// A <see cref="IConnectionProvider"/> that is used to create connections
    /// and query a SQL server.
    /// </param>
    public SqlQueryDataProvider(SqlConnectionProvider sql_connection_provider) {
#if DEBUG
      if (connection_provider == null)
        throw new ArgumentException("connection_provider");
#endif
      sql_connection_provider_ = sql_connection_provider;
      logger_ = RestQLLogger.ForCurrentProcess;
    }
    #endregion

    /// <inheritdoc/>
    public bool GetQuery(string name, out IQuery query) {
      using (SqlConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(sql_connection_provider_.Schema + kGetQuery)
          .SetType(CommandType.StoredProcedure)
          .AddParameter("@name", name, DbType.String, 260)
          .Build();

        try {
          conn.Open();
          using (IDataReader dr = cmd.ExecuteReader()) {
            query = CreatedQueryFromDataReader(dr);
            return true;
          }
        } catch (SqlException e) {
          logger_.Error(
            string.Format(StringResources.Log_MethodThrowsException, kClassName,
              "GetQuery"), e);
        }
      }
      query = null;
      return false;
    }

    public IProviderNode[] GetConnectionProviders() {
      using (SqlConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(sql_connection_provider_.Schema + kGetConnectionProviders)
          .SetType(CommandType.StoredProcedure)
          .Build();
        try {
          conn.Open();
        } catch (SqlException e) {
          logger_.Error(
            string.Format(StringResources.Log_MethodThrowsException, kClassName,
              "GetConnectionProviders"), e);
        }
      }
      return new IProviderNode[0];
    }

    /// <summary>
    /// Create an <see cref="Query"/> object by reading the data from the
    /// goven <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> object where the data to create the
    /// <see cref="Query"/> object could be read from.
    /// </param>
    /// <returns>
    /// A new <see cref="Query"/> object if the specified
    /// <see cref="IDataReader"/> is readable; otherwise the value of
    /// <see cref="Query.EmptyQuery"/> property.
    /// </returns>
    protected IQuery CreatedQueryFromDataReader(IDataReader reader) {
      if (reader.Read()) {
        int[] ordinals = reader
          .GetOrdinals("query_name", "query_type", "query", "query_method");

        const int kQueryName = 0;
        const int kQueryType = 1;
        const int kQuery = 2;
        const int kQueryMethod = 3;

        string name = reader.GetString(ordinals[kQueryName]);
        string type = reader.GetString(ordinals[kQueryType]);
        string query_string = reader.GetString(ordinals[kQuery]);
        int method = reader.GetInt32(ordinals[kQueryMethod]);

        var query = new Query(name, type, query_string) {
          QueryMethod = (QueryMethod) method
        };

        // get the query options
        if (reader.NextResult()) {
          SetQueryOptions(reader, query);
        }
        return query;
      }
      return Query.EmptyQuery;
    }

    void SetQueryOptions(IDataReader reader, Query query) {
      if (reader.Read()) {
        const int kQueryOptionName = 0;
        const int kQueryOptionValue = 1;

        int[] ordinals = reader.GetOrdinals("option_name", "option_value");
        do {
          string name = reader.GetString(ordinals[kQueryOptionName]);
          string value = reader.GetString(ordinals[kQueryOptionValue]);
          query.Options.Add(name, value);
        } while (reader.Read());
      }
    }
  }
}
