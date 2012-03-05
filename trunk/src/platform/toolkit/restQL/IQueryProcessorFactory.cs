using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A factory class that is used to create instances of classes that
  /// implements the <see cref="IQueryProcessor"/> interface.
  /// </summary>
  /// <remarks>
  /// This interface implies a constructor with no parameters.
  /// </remarks>
  public interface IQueryProcessorFactory
  {
    /// <summary>
    /// Creates a instance of the <see cref="IQueryProcessor"/> class by using
    /// the specified application settings.
    /// </summary>
    /// <param name="settings"></param>
    /// <returns>An instance of the <see cref="QueryProcessor"/> class.
    /// </returns>
    IQueryProcessor CreateQueryProcessor(RestQLSettings settings);
  }
}
