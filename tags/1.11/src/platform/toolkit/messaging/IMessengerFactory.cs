using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// Creates instances of classes that implement the <see cref="IMessenger"/>
  /// interface.
  /// </summary>
  /// <remarks>
  /// This interface implies a constructor that has no parameters.
  /// </remarks>
  public interface IMessengerFactory
  {
    /// <summary>
    /// Creates a instance of the <see cref="IMessenger"/> class.
    /// </summary>
    /// <param name="name">The string that identifies the messenger.</param>
    /// <param name="options">A <see cref="IDictionary&gt;stringm string&lt;"/>
    /// containing the options configured for this provider.</param>
    /// <remarks>
    /// The <paramref name="name"/> and <paramref name="options"/> parameters
    /// could not be null. If any parameter is null the constructor should
    /// throw an <see cref="ArgumentNullException"/> exception.
    /// <para>
    /// The factory should always return a valid <see cref="IMessenger"/>
    /// object. If a valid object could not be created a exception must be
    /// throwed. Note that null reference objects does not represent a valid
    /// <see cref="IMessenger"/> object.
    /// </para>
    /// </remarks>
    /// <returns>A instance of the <see cref="IMessenger"/> class.</returns>
    IMessenger CreateMessenger(string name,
      IDictionary<string, string> options);
  }
}