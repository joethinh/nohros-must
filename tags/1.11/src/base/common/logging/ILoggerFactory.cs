using System;
using System.Collections.Generic;
using Nohros.Configuration;
using Nohros.Providers;

namespace Nohros.Logging
{
  /// <summary>
  /// A factory used to create instances of the <see cref="ILogger"/> class.
  /// </summary>
  /// <remarks>
  /// This interfaces implies a constructor taht receive no parameters or
  /// a constructor that receives a parameter of type
  /// <see cref="Nohros.Configuration.IConfiguration"/>.
  /// <para>
  /// When instances of the <see cref="ILoggerFactory"/> is dynamically created
  /// you need to try to build it using the constructor that receives a
  /// parameter of type <see cref="Nohros.Configuration.IConfiguration"/> first, and if it fails
  /// falls back to the constructor that receives no parameters.
  /// </para>
  /// </remarks>
  public interface ILoggerFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="ILogger"/> class using the
    /// specified provider node.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> object that contains the
    /// options for the logger to be created.
    /// </param>
    /// <returns>
    /// The newly created <see cref="ILogger"/> object.
    /// </returns>
    ILogger CreateLogger(IDictionary<string, string> options);
  }
}
