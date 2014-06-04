using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Resources;

namespace Nohros
{
    /// <summary>
    /// Logical representation of a pointer, but in fact a byte array reference and a position in it. This
    /// is used to read logical units (bytes, shorts, integers, domain names etc.) from a byte array, keeping
    /// the pointer updated and positioning to the next record. This type of pointer can be considered the logical
    /// equivalent of an (unsigned char*) in C++;
    /// </summary>
    public class Pointer : IPointer
    {
        /// <summary>
        /// The pointer message.
        /// </summary>
        protected byte[] message_;

        /// <summary>
        /// The message length.
        /// </summary>
        protected int length_;

        /// <summary>
        /// An index within the [message] representing the current pointer position.
        /// </summary>
        protected int position_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the Pointer class by using the specified message and position.
        /// </summary>
        /// <param name="message">An sequence of bytes that represents/contains the pointer.</param>
        /// <param name="position">An index within the <paramref name="message"/> the is the begging of the pointer.</param>
        public Pointer(byte[] message, int position) {
            if (message == null)
                throw new ArgumentNullException("message");

            if (position < 0 || position >= message.Length)
                throw new IndexOutOfRangeException(StringResources.Arg_IndexOutOfRange);

            message_ = message;
            position_ = position;
            length_ = message_.Length;
        }
        #endregion

        /// <summary>
        /// Creates and returns an identical copy of the current pointer object.
        /// </summary>
        /// <returns>A copy of the current pointer.</returns>
        public virtual Pointer Copy() {
            return new Pointer(message_, position_);
        }

        /// <summary>
        /// Overloads the + operator to allow advancing the pointer by so many bytes.
        /// </summary>
        /// <param name="pointer">the initial pointer.</param>
        /// <param name="offset">the offset to add to the pointer in bytes.</param>
        /// <returns>a reference to a new pointer moved forward by offset bytes.</returns>
        /// <exception cref="IndexOutOfRangeException">Advancing the pointer by <paramref name="offset"/> bytes
        /// past the related message array bounds.</exception>
        public static Pointer operator +(Pointer pointer, int offset) {
            int position = pointer.position_ + offset;
            if (position < 0 || position >= pointer.message_.Length)
                throw new IndexOutOfRangeException(StringResources.Arg_IndexOutOfRange);

            pointer.position_ = position;

            return pointer;
        }

        /// <summary>
        /// Overloads the ++ operator to allow pointer increment.
        /// </summary>
        /// <param name="pointer">the initial pointer.</param>
        /// <returns>a reference to a new pointer moved backward by one byte.</returns>
        /// <exception cref="IndexOutOfRangeException">Advancing the pointer by <paramref name="offset"/> bytes
        /// past the related message array bounds.</exception>
        public static Pointer operator ++(Pointer pointer) {
            return (pointer + 1);
        }

        /// <summary>
        /// Overloads the - operator to allow advancing the pointer by so many bytes.
        /// </summary>
        /// <param name="pointer">the initial pointer.</param>
        /// <param name="offset">the offset to add to the pointer in bytes.</param>
        /// <returns>a reference to a new pointer moved forward by offset bytes.</returns>
        /// <exception cref="IndexOutOfRangeException">Advancing the pointer by <paramref name="offset"/> bytes
        /// past the related message array bounds.</exception>
        public static Pointer operator -(Pointer pointer, int offset) {
            int position = pointer.position_ - offset;
            if (position < 0 || position >= pointer.message_.Length)
                throw new IndexOutOfRangeException(StringResources.Arg_IndexOutOfRange);

            pointer.position_ = position;

            return pointer;
        }

        /// <summary>
        /// Overloads the -- operator to allow pointer decrement.
        /// </summary>
        /// <param name="pointer">the initial pointer.</param>
        /// <returns>a reference to a new pointer moved backward by one byte.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Moving the pointer by one byte
        /// back past the related message array bounds.</exception>
        public static Pointer operator --(Pointer pointer) {
            return (pointer - 1);
        }

        /// <summary>
        /// Converts the underlying message to its equivalent System.String representation encoded with base 64 digits.
        /// </summary>
        /// <param name="pointer">A <see cref="Pointer"/>object.</param>
        /// <returns>The string representation, in base 64, of the underlying message.</returns>
        /// <remarks>
        /// The bytes of the underlying message ate taken as a numeric value and converted to a string representation that
        /// is encoded with base-64 digits.
        /// <para>
        /// The base-64 digits in ascending order from zero are the upercase characters "A" to "Z", the lowercase characters
        /// "a" to "z", the numerals "0" to "9", and the symbols "+" and "/". The valueless character "=", is used for tailing
        /// padding.
        /// </para>
        /// </remarks>
        public static string ToBase64String(Pointer pointer) {
            byte[] message = pointer.message_;
            int message_length = message.Length;
            byte[] pre_encoded_message = new byte[message_length + 4];

            Buffer.BlockCopy(message, 0, pre_encoded_message, 0, message_length);

            // the last four bytes of the pre encoded array will be filled with
            // the current pointer position.
            pre_encoded_message[message_length] = (byte)(pointer.position_ & 0xf000);
            pre_encoded_message[message_length + 1] = (byte)(pointer.position_ & 0xf00);
            pre_encoded_message[message_length + 2] = (byte)(pointer.position_ & 0xf0);
            pre_encoded_message[message_length + 3] = (byte)(pointer.position_ & 0xf);

            return Convert.ToBase64String(pre_encoded_message);
        }

        /// <summary>
        /// Converts the specified string, which encodes a Pointer object as base-64 digits, to an
        /// equivalent Pointer object.
        /// </summary>
        /// <param name="s">The string representation of a pointer object to convert.</param>
        /// <returns>A pointer object that is equivalent to <paramref name="s"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="FormatException">The length of <paramref name="s"/>, ignoring white-space characters, is not zero or
        /// a multiple of 4. -or- The format of <paramref name="s"/> is invalid. <paramref name="s"/> contains a non-base-64 character
        /// , more than two padding characters, or a non-white space-cheracter among the padding characters.</exception>
        /// <remarks>
        /// <paramref name="s"/> is composed of base-64 digits, white-space characters, and trailing padding characters.
        /// The base-64 digits in ascending order from zero are the upercase characters "A" to "Z", the lowercase characters
        /// "a" to "z", the numerals "0" to "9", and the symbols "+" and "/".
        /// <para>
        /// The white-space character, and their Unicode names and hexadecimal code points, are tab(CHARACTER TABULATION, U+0009),
        /// newline(LINE FEED, U+000A), carriage return(CARRIAGE RETURN, U+000D), and blank(SPACE, U+0020). An arbitrary number
        /// of white spaces characters can appear in <paramref name="s"/> because all white-space characters are ignored.
        /// </para>
        /// <para>
        /// The valueless character "=", is used for tailing padding. The end of <paramref name="s"/> can consist of zero, one,
        /// or two padding characters.
        /// </para>
        /// </remarks>
        public static Pointer FromBase64String(string s) {
            // let the Convert function do the checks for us.
            byte[] pre_decoded_message = Convert.FromBase64String(s);

            // the last four bytes represents the position of the pointer
            // when it was encoded.
            int message_len = pre_decoded_message.Length - 4;
            byte[] message = new byte[message_len];

            // separate the wheat(message) from the chaff(pointer position)
            int position = (ushort)((pre_decoded_message[message_len] << 8 | pre_decoded_message[message_len + 1]) << 16) |
                (ushort)(pre_decoded_message[message_len + 2] << 8 | pre_decoded_message[message_len + 3]);
            Buffer.BlockCopy(pre_decoded_message, 0, message, 0, message_len);

            return new Pointer(message, position);
        }

        /// <summary>
        /// Reads a single byte at the current position, advancing the pointer.
        /// </summary>
        /// <returns>the byte at the current position.</returns>
        /// <remarks>If the current position is the end of the message array, the pointer will not be advanced and
        /// the last byte will be returned.</remarks>
        /// <exception cref="IndexOutOfRangeException">Advancing the pointer past the bounds of the underlying message
        /// array.</exception>
        public byte GetByte() {
            // let the .NET framework do the bound checks and exception throwing for us.
            return message_[position_++];
        }

        /// <summary>
        /// Reads a single short(2 bytes) at the current position, advancing pointer.
        /// </summary>
        /// <returns>the short at the current position</returns>
        /// <exception cref="IndexOutOfRangeException">Advancing the pointer past the bounds of the underlying message
        /// array.</exception>
        public short GetShort() {
            return (short)(GetByte() << 8 | GetByte());
        }

        /// <summary>
        /// Reads a single int(4 bytes) from the current position, advancing the pointer.
        /// </summary>
        /// <returns>the int at the current position.</returns>
        /// <exception cref="IndexOutOfRangeException">Advancing the pointer past the bounds of the underlying message
        /// array.</exception>
        public int GetInt() {
            return (ushort)GetShort() << 16 | (ushort)GetShort();
        }

        /// <summary>
        /// Reads a single byte as a char at the current position, advancing the pointer.
        /// </summary>
        /// <returns>the char at the current position.</returns>
        /// <exception cref="IndexOutOfRangeException">Advancing the pointer past the bounds of the underlying message
        /// array.</exception>
        public char GetChar() {
            return (char)GetByte();
        }

        /// <summary>
        /// Reads a single byte at the current pointer, does not advance pointer
        /// </summary>
        /// <returns>the byte at the current position.</returns>
        public byte Peek() {
            return this[position_];
        }

        /// <summary>
        /// Reads a single byte at the specified position, does not advance the pointer.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The byte at the specified position.</returns>
        ///<exception cref="IndexOutOfRangeException">The specified position is outside the bounds of the underlying message array.</exception>
        public byte this[int position] {
            get {
                if (position < 0 || position > length_)
                    throw new IndexOutOfRangeException(StringResources.Arg_IndexOutOfRange);
                return message_[position];
            }
        }

        /// <summary>
        /// Gets the current pointer position.
        /// </summary>
        public int Position {
            get { return position_; }
        }

        /// <summary>
        /// Gets the underlying message as a byte array.
        /// </summary>
        public byte[] Message {
            get { return message_; }
        }
    }
}
