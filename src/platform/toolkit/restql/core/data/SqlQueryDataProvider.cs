using System;
using System.Data;
using System.Data.SqlClient;
using Nohros.Data;
using Nohros.Data.Providers;
using Nohros.Extensions;
using Nohros.Resources;

namespace Nohros.RestQL
{
  public class SqlQueryDataProvider : IQueryDataProvider
  {
    const string kClassName = "Nohros.RestQL.SqlQueryDataProvider";

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
      if (sql_connection_provider == null)
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
            query = CreateQueryFromDataReader(dr);
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
    protected IQuery CreateQueryFromDataReader(IDataReader reader) {
      if (reader.Read()) {
        int[] ordinals = reader
          .GetOrdinals("query_name", "query_type", "query", "query_method",
            "query_use_space_as_terminator", "query_delimiter");

        const int kQueryName = 0;
        const int kQueryType = 1;
        const int kQuery = 2;
        const int kQueryMethod = 3;
        const int kUseSpaceTerminator = 4;
        const int kQueryDelimiter = 5;

        bool use_space_terminator =
          reader.GetBoolean(ordinals[kUseSpaceTerminator]);
        string delimiter = reader.GetString(ordinals[kQueryDelimiter]);
        string name = reader.GetString(ordinals[kQueryName]);
        string type = reader.GetString(ordinals[kQueryType]);
        string query_string = reader.GetString(ordinals[kQuery]);
        int method = reader.GetInt32(ordinals[kQueryMethod]);

        var query = new Query(name, type, query_string, delimiter) {
          QueryMethod = (QueryMethod) method,
          UseSpaceAsTerminator = use_space_terminator
        };

        // get the query options
        if (reader.NextResult()) {
          SetQueryOptions(reader, query);
        }
        query.Parse();
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
