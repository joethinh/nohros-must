using System;
using System.Collections.Generic;
using NUnit.Framework;
using Telerik.JustMock;

namespace Nohros.Toolkit.RestQL
{
  public class QueryResolverTests
  {
    QueryResolver GetQueryResolver() {
      var query = Mock.Create<IQuery>();
      Mock
        .Arrange(() => query.Name)
        .Returns("query");

      var executor = Mock.Create<IQueryExecutor>();
      Mock
        .Arrange(query, q => executor.CanExecute(q))
        .Returns(true);
      Mock
        .Arrange(() => executor
          .Execute(query, Arg.IsAny<IDictionary<string, string>>()))
        .Returns("result");

      var query_data_provider = Mock.Create<IQueryDataProvider>();
      Mock
        .Arrange(() => query_data_provider.GetQuery(Arg.IsAny<string>()))
        .Returns(Query.EmptyQuery);
      Mock
        .Arrange(() => query_data_provider.GetQuery("query"))
        .Returns(query);

      return new QueryResolver(new[] {executor}, query_data_provider);
    }

    [Test]
    public void ShouldResolveQueryUsingItsName() {
      QueryResolver resolver = GetQueryResolver();
      IQuery resolved_query = resolver.GetQuery("query");
      Assert.That(resolved_query.Name, Is.EqualTo("query"));
    }

    [Test]
    public void ShouldReturnEmptyQueryWhenNameIsNotFound() {
      var resolver = GetQueryResolver();
      IQuery query = resolver.GetQuery("missing");
      Assert.That(query, Is.EqualTo(Query.EmptyQuery));
    }

    [Test]
    public void ShoulReturnNoOpQueryExecutorWhenExecutorIsNotFound() {
      QueryResolver resolver = GetQueryResolver();
      IQueryExecutor executor = resolver.GetQueryExecutor(Query.EmptyQuery);
      Assert.That(executor,
        Is.EqualTo(NoOpQueryExecutor.StaticNoOpQueryExecutor));
    }
  }
}
