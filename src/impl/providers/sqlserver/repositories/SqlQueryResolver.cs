using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Nohros.Data.SqlServer
{
  public static class SqlQueryResolver
  {
    static void Join(IEnumerable<string> strs, string separator,
      StringBuilder str) {
      foreach (string field in strs) {
        str
          .Append(field)
          .Append(separator);
      }
      if (str.Length != 0) {
        str.Remove(str.Length - separator.Length, separator.Length);
      }
    }

    /// <summary>
    /// Resolve the given <paramref name="criteria"/> into a
    /// <see cref="CommandBuilder"/> object.
    /// </summary>
    /// <param name="from_clause"></param>
    /// <param name="connection"></param>
    /// <param name="criteria"></param>
    /// <returns></returns>
    public static CommandBuilder Resolve(SqlConnection connection,
      string from_clause, ICriteria criteria) {
      var builder = new CommandBuilder(connection);
      Resolve(builder, from_clause, criteria);
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
    public static CommandBuilder Resolve(SqlCommand cmd, string from_clause,
      ICriteria criteria) {
      var builder = new CommandBuilder(cmd);
      Resolve(builder, from_clause, criteria);
      return builder;
    }

    /// <summary>
    /// Resolve the given <paramref name="criteria"/> into a
    /// <see cref="CommandBuilder"/> object.
    /// </summary>
    public static void Resolve(CommandBuilder cmd, string from_clause,
      ICriteria criteria) {
      var str = new StringBuilder();

      if (criteria.Fields.Count != 0) {
        str.Append("select ");
        Join(criteria.Fields, ",", str);

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
  }
}
