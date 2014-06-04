using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Nohros.Extensions;

namespace Nohros.Data
{
  /// <summary>
  /// Resolve <see cref="ICriteria"/> to other related objects.
  /// </summary>
  public static class CriteriaResolver
  {
    static readonly IDictionary<string, object> mappers_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="CriteriaResolver"/> class.
    /// </summary>
    static CriteriaResolver() {
      mappers_ = new Dictionary<string, object>();
    }
    #endregion

    /// <summary>
    /// Resolve the given <paramref name="criteria"/> into a
    /// <see cref="CommandBuilder"/> object.
    /// </summary>
    /// <param name="from_clause"></param>
    /// <param name="connection"></param>
    /// <param name="criteria"></param>
    /// <returns></returns>
    public static CommandBuilder Resolve(this ICriteria criteria,
      string from_clause, IDbConnection connection) {
      var builder = new CommandBuilder(connection);
      criteria.Resolve(from_clause, builder);
      return builder;
    }

    /// <summary>
    /// Resolve the given <paramref name="criteria"/> into a
    /// <see cref="CommandBuilder"/> object.
    /// </summary>
    /// <remarks>
    /// The <see cref="SqlCommand.CommandText"/>,
    /// <see cref="SqlCommand.CommandType"/> properties of the
    /// <paramref name="cmd"/> will be modified. The parameters
    /// that is already present in the <see cref="IDbCommand.Parameters"/>
    /// will not be removed, but new parameters will be added.
    /// </remarks>
    public static CommandBuilder Resolve(this ICriteria criteria,
      string from_clause, IDbCommand cmd) {
      var builder = new CommandBuilder(cmd);
      criteria.Resolve(from_clause, builder);
      return builder;
    }

    /// <summary>
    /// Resolve the given <paramref name="criteria"/> into a
    /// <see cref="CommandBuilder"/> object.
    /// </summary>
    public static void Resolve(this ICriteria criteria, string from_clause,
      CommandBuilder cmd) {
      var str = new StringBuilder();

      if (criteria.Fields.Count != 0) {
        str.Append("select ");
        criteria.Fields.Join(",", str);

        str.Append(" ").Append(from_clause);
        if (criteria.Filters.Count != 0) {
          str.Append(" where ");

          foreach (KeyValuePair<string, object> filter in criteria.Filters) {
            string parm = "@" + filter.Key;
            cmd.AddParameterWithValue(parm, filter.Value);
            str
              .Append(filter.Key)
              .Append("=")
              .Append(parm);
          }
        }
      }

      cmd.SetText(str.ToString())
         .SetType(CommandType.Text);
    }

    /// <summary>
    /// Resolve the given <paramref name="criteria"/> into a
    /// <see cref="IDataReaderMapper{T}"/> object.
    /// </summary>
    public static IDataReaderMapper<T> Resolve<T>(this ICriteria criteria,
      string prefix) {
      object obj;
      string hash = prefix + ":" + criteria.Fields.Join(":");
      if (mappers_.TryGetValue(hash, out obj)) {
        return (IDataReaderMapper<T>) obj;
      }

      var builder = new DataReaderMapperBuilder<T>();
      foreach (var field in criteria.Fields) {
        string map = criteria.GetFieldMap(field);
        builder.Map(field, map);
      }
      IDataReaderMapper<T> mapper = builder.Build();
      mappers_.Add(hash, mapper);
      return mapper;
    }
  }
}
