using System;

namespace Nohros.Toolkit.RestQL
{
  public partial class QueryResolver
  {
    internal class QueryExecutorPair {
      readonly Query query_;
      IQueryExecutor query_executor_;

      public QueryExecutorPair(Query query) {
        query_ = query;
        query_executor_ = null;
      }

      public Query Query {
        get { return query_; }
      }

      public IQueryExecutor QueryExecutor {
        get { return query_executor_; }
        set { query_executor_ = value; }
      }
    }
  }
}
