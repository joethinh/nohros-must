using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Resources;

namespace Nohros
{
  /// <summary>
  /// Logical representation of a pointer, but in fact a byte array reference
  /// and a position in it. This is used to read logical units (bytes, shorts,
  /// integers, domain names etc.) from a byte array, keeping the pointer
  /// updated and positioning to the next record. This type of pointer can be
  /// considered the logical equivalent of an (unsigned char*) in C++;
  /// </summary>
  public interface IPointer
  {
    /// <summary>
    /// Creates and returns an identical copy of the current permission.
    /// </summary>
    /// <returns>A copy of the current pointer.</returns>
    Pointer Copy();

    /// <summary>
    /// Reads a single byte at the current position, advancing the pointer.
    /// </summary>
    /// <returns>the byte at the current position.</returns>
    /// <remarks>If the current position is the end of the message array, the pointer will not be advanced and
    /// the last byte will be returned.</remarks>
    /// <exception cref="IndexOutOfRangeException">Advancing the pointer past the bounds of the underlying message
    /// array.</exception>
    byte GetByte();

    /// <summary>
    /// Reads a single short(2 bytes) at the current position, advancing pointer.
    /// </summary>
    /// <returns>the short at the current position</returns>
    /// <exception cref="IndexOutOfRangeException">Advancing the pointer past the bounds of the underlying message
    /// array.</exception>
    short GetShort();

    /// <summary>
    /// Reads a single int(4 bytes) from the current position, advancing the pointer.
    /// </summary>
    /// <returns>the int at the current position.</returns>
    /// <exception cref="IndexOutOfRangeException">Advancing the pointer past the bounds of the underlying message
    /// array.</exception>
    int GetInt();

    /// <summary>
    /// Reads a single byte as a char at the current position, advancing the pointer.
    /// </summary>
    /// <returns>the char at the current position.</returns>
    /// <exception cref="IndexOutOfRangeException">Advancing the pointer past the bounds of the underlying message
    /// array.</exception>
    char GetChar();

    /// <summary>
    /// Reads a single byte at the current pointer, does not advance pointer
    /// </summary>
    /// <returns>the byte at the current position.</returns>
    byte Peek();

    /// <summary>
    /// Reads a single byte at the specified position, does not advance the pointer.
    /// </summary>
    /// <param name="position"></param>
    /// <returns>The byte at the specified position.</returns>
    ///<exception cref="IndexOutOfRangeException">The specified position is outside the bounds of the underlying message array.</exception>
    byte this[int position] { get; }

    /// <summary>
    /// Gets the current pointer position.
    /// </summary>
    int Position { get; }
  }
}
