using System;

namespace Nohros.Toolkit.RestQL
{
  public partial class QueryResolver
  {
    internal class QueryExecutorPair {
      readonly IQuery query_;
      IQueryExecutor query_executor_;

      public QueryExecutorPair(IQuery query) {
        query_ = query;
        query_executor_ = null;
      }

      public IQuery Query {
        get { return query_; }
      }

      public IQueryExecutor QueryExecutor {
        get { return query_executor_; }
        set { query_executor_ = value; }
      }
    }
  }
}
