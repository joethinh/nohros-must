using System;
using System.Data;
using Nohros.Configuration;
using Nohros.Data;

namespace Nohros.Toolkit.RestQL
{
  public abstract class QueryDataProvider : IQueryDataProvider
  {
    const int kQueryName = 0;
    const int kQueryType = 1;
    const int kQuery = 2;
    const int kQueryMethod = 3;

    const int kQueryOptionName = 0;
    const int kQueryOptionValue = 1;

    protected readonly string[] get_query_fields = {
      "queryname", "querytype", "query", "querymethod"
    };

    protected readonly string[] get_query_options_fields = {
      "optionname", "optionvalue"
    };

    #region IQueryDataProvider Members
    /// <inheritdoc/>
    public abstract IQuery GetQuery(string name);

    /// <inheritdoc/>
    public abstract IProviderNode[] GetConnectionProviders();
    #endregion

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
        int[] ordinals = DataReaders.GetOrdinals(reader, get_query_fields);

        string name = reader.GetString(ordinals[kQueryName]);
        string type = reader.GetString(ordinals[kQueryType]);
        string query_string = reader.GetString(ordinals[kQuery]);
        int method = reader.GetInt32(ordinals[kQueryMethod]);

        Query query = new Query(name, type, query_string);
        query.QueryMethod = (QueryMethod) method;

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
        int[] ordinals = DataReaders.GetOrdinals(reader,
          get_query_options_fields);
        do {
          string name = reader.GetString(ordinals[kQueryOptionName]);
          string value = reader.GetString(ordinals[kQueryOptionValue]);
          query.Options.Add(name, value);
        } while (reader.Read());
      }
    }
  }
}
