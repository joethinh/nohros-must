using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Nohros.Data;
using Nohros.Data.Providers;

namespace Nohros.Toolkit.RestQL
{
  public abstract class CommonDataProvider : ICommonDataProvider
  {
    protected readonly string[] get_query_fields = {
      "queryname", "querytype" ,"query" };

    /// <summary>
    /// Create an <see cref="Query"/> object by reading the data from the
    /// goven <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="data_reader">
    /// A <see cref="IDataReader"/> object where the data to create the
    /// <see cref="Query"/> object could be read from.
    /// </param>
    /// <returns>
    /// A new <see cref="Query"/> object if the specified
    /// <see cref="IDataReader"/> is readable; otherwise the value of
    /// <see cref="Query.EmptyQuery"/> property.
    /// </returns>
    protected Query CreatedQueryFromDataReader(IDataReader data_reader) {
      if (data_reader.Read()) {
        int[] ordinals = DataReaders.GetOrdinals(data_reader, get_query_fields);

        const int kQueryName = 0;
        const int kQueryProcessor = 2;
        const int kQuery = 3;

        string name = data_reader.GetString(ordinals[kQueryName]);
        string type = data_reader.GetString(ordinals[kQueryProcessor]);
        string query = data_reader.GetString(ordinals[kQuery]);

        return new Query(name, type, query);
      }
      return Query.EmptyQuery;
    }

    /// <inheritdoc/>
    public abstract Query GetQuery(string name);
  }
}