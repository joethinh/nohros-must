using System;
using System.Globalization;

namespace Nohros.Data
{
  /// <summary>
  /// Represents a query that gets a state that is associated with a given
  /// name.
  /// </summary>
  [Obsolete("This interface is obsolete. Check the IStateDao interface.")]
  public interface IStateByNameQuery
  {
    /// <summary>
    /// Gets the state that is associate with the given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the state to get.
    /// </param>
    /// <returns>
    /// The state that is associated with the given <paramref name="name"/>.
    /// </returns>
    /// <exception cref="NoResultException">
    /// There is no state associated with the given <paramref name="name"/>.
    /// </exception>
    string Execute(string name);

    /// <summary>
    /// Try to get the state that is associated with the given
    /// <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the state to get.
    /// </param>
    /// <param name="state">
    /// When this method returns contains the state that is associated with the
    /// given <paramref name="name"/>; or <c>null</c> if  <paramref name="name"/>
    /// is not found.
    /// </param>
    /// <returns>
    /// <c>true</c> is a <paramref name="name"/> is found; otherwise
    /// <c>false</c>.
    /// </returns>
    bool Execute(string name, out string state);

    /// <summary>
    /// Try to get the state that is associated with the given
    /// <paramref name="name"/> and convert it to an signed 32-bit integer.
    /// </summary>
    /// <param name="name">
    /// The name of the state to get.
    /// </param>
    /// <param name="state">
    /// When this method returns contains the state that is associated with the
    /// given <paramref name="name"/>; or <c>-1</c> if  <paramref name="name"/>
    /// is not found.
    /// </param>
    /// <returns>
    /// <c>true</c> is a <paramref name="name"/> is found; otherwise
    /// <c>false</c>.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// The state associated with <paramref name="name"/> cannot be converted
    /// to an signed 32-bit integer value.
    /// </exception>
    bool Execute(string name, out int state);

    /// <summary>
    /// Try to get the state that is associated with the given
    /// <paramref name="name"/> and convert it to a <see cref="Guid"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the state to get.
    /// </param>
    /// <param name="state">
    /// When this method returns contains the state that is associated with the
    /// given <paramref name="name"/>; or <see cref="Guid.Empty"/>
    /// <paramref name="name"/> is not found.
    /// </param>
    /// <returns>
    /// <c>true</c> is a <paramref name="name"/> is found; otherwise
    /// <c>false</c>.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// The state associated with <paramref name="name"/> cannot be converted
    /// to an <see cref="Guid"/>
    /// </exception>
    bool Execute(string name, out Guid state);

    /// <summary>
    /// Try to get the state that is associated with the given
    /// <paramref name="name"/> and convert it to a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the state to get.
    /// </param>
    /// <param name="state">
    /// When this method returns contains the state that is associated with the
    /// given <paramref name="name"/>; or <see cref="DateTime.MinValue"/> if
    /// <paramref name="name"/> is not found.
    /// </param>
    /// <returns>
    /// <c>true</c> is a <paramref name="name"/> is found; otherwise
    /// <c>false</c>.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// The state associated with <paramref name="name"/> cannot be converted
    /// to an <see cref="DateTime"/>.
    /// </exception>
    bool Execute(string name, out DateTime state);

    /// <summary>
    /// Try to get the state that is associated with the given
    /// <paramref name="name"/> and convert it to a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the state to get.
    /// </param>
    /// <param name="state">
    /// When this method returns contains the state that is associated with the
    /// given <paramref name="name"/>; or <see cref="DateTime.MinValue"/> if
    /// <paramref name="name"/> is not found.
    /// </param>
    /// <param name="styles">
    /// A bitwise combination of the enumeration values that indicates the
    /// style elements that can be present in the "state" for the parse
    /// operation to succeed, and that defines how to interpret the parsed
    /// date in relation to the current time zone or the current date. A
    /// typical value to specify is <see cref="DateTimeStyles.None"/>
    /// </param>
    /// <returns>
    /// <c>true</c> is a <paramref name="name"/> is found; otherwise
    /// <c>false</c>.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// The state associated with <paramref name="name"/> cannot be converted
    /// to an <see cref="DateTime"/>.
    /// </exception>
    bool Execute(string name, DateTimeStyles styles, out DateTime state);
  }
}
