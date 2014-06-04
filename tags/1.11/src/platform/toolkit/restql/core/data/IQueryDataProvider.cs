using System;
using Nohros.Configuration;

namespace Nohros.RestQL
{
  public interface IQueryDataProvider
  {
    /// <summary>
    /// Gets a <see cref="Query"/> object whose name if <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the query to get.
    /// </param>
    /// <returns>
    /// A <see cref="Query"/> object whose name is <paramref name="name"/>.
    /// </returns>
    /// <remarks>
    /// This method should not throw exceptions. If a <see cref="Query"/> with
    /// name does not exists in the database this method should return an
    /// instance of the class <see cref="Query.EmptyQuery"/>.
    /// </remarks>
    bool GetQuery(string name, out IQuery query);
  }
}
