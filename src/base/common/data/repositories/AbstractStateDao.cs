using System;
using System.Globalization;

namespace Nohros.Data
{
  /// <summary>
  /// Provides an skeletal implementation of the
  /// <see cref="IStateDao"/> interface to minimize the effort required
  /// to implement this interface.
  /// </summary>
  /// <remarks>
  /// To implement a <see cref="IStateDao"/>, its required only to
  /// extend this class and provide an implementation for the
  /// <see cref="StateByName(string, out string)"/> and <see cref="SetState()"/>]
  /// methods.
  /// </remarks>
  public abstract class AbstractStateDao : IStateDao
  {
    /// <inheritdoc/>
    public virtual string StateByName(string name) {
      string s;
      if (StateByName(name, out s)) {
        return s;
      }
      throw new NoResultException();
    }

    /// <inheritdoc/>
    public abstract void SetState(string name, string state);

    /// <inheritdoc/>
    public abstract bool StateByName(string name, out string state);

    /// <remarks>
    /// This method uses the <see cref="int.Parse(string)"/> method to convert
    /// the "state" into its corresponding signed 32-bit integer. Exceptions
    /// throwed by this method will be propagated to the caller.
    /// </remarks>
    public virtual bool StateByName(string name, out int state) {
      string s;
      if (StateByName(name, out s)) {
        state = int.Parse(s);
        return true;
      }
      state = -1;
      return false;
    }

    /// <remarks>
    /// This method uses the <see cref="Guid(string)"/> method to convert the
    /// "state" into its <see cref="Guid"/>. Exceptions throwed by this method
    /// will be propagated to the caller.
    /// </remarks>
    public virtual bool StateByName(string name, out Guid state) {
      string s;
      if (StateByName(name, out s)) {
        state = new Guid(s);
        return true;
      }
      state = Guid.Empty;
      return false;
    }

    /// <remarks>
    /// This method uses the <see cref="DateTime.Parse(string)"/> method to
    /// convert the "state" into its <see cref="Guid"/>. Exceptions throwed by
    /// this method will be propagated to the caller.
    /// </remarks>
    public virtual bool StateByName(string name, out DateTime state) {
      string s;
      if (StateByName(name, out s)) {
        state = DateTime.Parse(s);
        return true;
      }
      state = DateTime.MinValue;
      return false;
    }

    /// <remarks>
    /// This method uses the
    /// <see cref="DateTime.Parse(string,IFormatProvider,DateTimeStyles)"/>
    /// method to convert the "state" into its <see cref="Guid"/>. Exceptions
    /// throwed by this method will be propagated to the caller.
    /// </remarks>
    public virtual bool StateByName(string name, DateTimeStyles styles,
      out DateTime state) {
      string s;
      if (StateByName(name, out s)) {
        state = DateTime.Parse(s, null, styles);
        return true;
      }
      state = DateTime.MinValue;
      return false;
    }
  }
}
