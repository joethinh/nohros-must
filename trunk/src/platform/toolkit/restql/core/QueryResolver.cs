using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Caching;
using Nohros.Collections;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// The default implementation of the <see cref="IQueryResolver"/> interface.
  /// </summary>
  public class QueryResolver : IQueryResolver
  {
    readonly ILoadingCache<Query> cache_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryResolver"/> class,
    /// using the specified cache.
    /// </summary>
    /// <param name="cache">
    /// A <see cref="ILoadingCache{T}"/> that is used to cache the resolved
    /// queries.
    /// </param>
    public QueryResolver(ILoadingCache<Query> cache) {
      cache_ = cache;
    }
    #endregion

    #region IQueryResolver Members
    /// <inheritdoc/>
    public Query GetQuery(string name) {
      Query query = cache_.Get(name);
      query.Parse();
      return query;
    }

    /// <inheritdoc/>
    public Query GetQuery(string name, IDictionary<string, string> parameters) {
      Query query = cache_.Get(name);
      query.Parse();

      ParameterizedStringPartParameterCollection query_parameters =
        query.Parameters;
      foreach (string parameter in parameters.Keys) {
        int index = query_parameters.IndexOf(parameter);
        if (index == -1) {
          if (MustLogger.ForCurrentProcess.IsWarnEnabled) {
            MustLogger.ForCurrentProcess.Warn("The parameter "
              + parameter + " is not recognized by the query "
              + query.Name);
          }
          return Query.EmptyQuery;
        }
        query_parameters[index].Value = parameter;
      }
      return query;
    }
    #endregion
  }
}
