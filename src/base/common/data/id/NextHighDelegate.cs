using System;

namespace Nohros.Data
{
  /// <summary>
  /// A method that is called to get the next High value for the given key.
  /// </summary>
  /// <param name="key">
  /// A string that identifies the object to get the next high value(ex. The
  /// name of a table in a RDBMS).
  /// </param>
  public delegate IHiLoRange NextHighDelegate(string key);
}
