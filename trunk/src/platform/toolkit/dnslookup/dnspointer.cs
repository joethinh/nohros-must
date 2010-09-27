using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Data;

namespace Nohros.Toolkit.DnsLookup
{
    public class RecordPointer : Pointer, IPointer
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the RecordPointer class by using the specified message, position and length.
        /// </summary>
        /// <param name="message">The pointer message.</param>
        /// <param name="position">The position within the message that is the begging of the position.</param>
        /// <param name="length">The length of the message related with the pointer.</param>
        public RecordPointer(byte[] message, int position):base(message, position) { }
        #endregion

        /// <summary>
        /// Overloads the + operator to allow advancing the pointer by so many bytes.
        /// </summary>
        /// <param name="pointer">the initial pointer.</param>
        /// <param name="offset">the offset to add to the pointer in bytes.</param>
        /// <returns>a reference to a new pointer moved forward by offset bytes.</returns>
        /// <exception cref="IndexOutOfRangeException">Advancing the pointer by <paramref name="offset"/> bytes
        /// past the related message array bounds.</exception>
        public static RecordPointer operator +(RecordPointer pointer, int offset) {
            return (RecordPointer)(((Pointer)pointer) + offset);
        }

        /// <summary>
        /// Overloads the ++ operator to allow pointer increment.
        /// </summary>
        /// <param name="pointer">the initial pointer.</param>
        /// <returns>a reference to a new pointer moved backward by one byte.</returns>
        /// <exception cref="IndexOutOfRangeException">Advancing the pointer by <paramref name="offset"/> bytes
        /// past the related message array bounds.</exception>
        public static RecordPointer operator ++(RecordPointer pointer) {
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
        public static RecordPointer operator -(RecordPointer pointer, int offset) {
            return (RecordPointer)(((Pointer)pointer) - offset);
        }

        /// <summary>
        /// Overloads the -- operator to allow pointer decrement.
        /// </summary>
        /// <param name="pointer">the initial pointer.</param>
        /// <returns>a reference to a new pointer moved backward by one byte.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Moving the pointer by one byte
        /// back past the related message array bounds.</exception>
        public static RecordPointer operator --(RecordPointer pointer) {
            return (pointer - 1);
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
        public new static RecordPointer FromBase64String(string s) {
            Pointer pointer = Pointer.FromBase64String(s);
            return new RecordPointer(pointer.Message, pointer.Position);
        }

        /// <summary>
        /// Reads a domain name from the byte array(RFC1035 4.1.4).
        /// </summary>
        /// <returns>An string representing a DNS domain.</returns>
        /// <remarks>In order to reduce the size of messages, the domain system utilizes a
        /// compression shceme which eliminates the repetition of a domain names in a message.
        /// In this scheme, an entire domain name or a list of labels at the end of a domain name
        /// is replaced with a prior occurrence of the same name. The compression scheme allows
        /// a domain name in a message to be represented as either:
        ///   . a sequence of labels ending in a zero octet.
        ///   . a pointer.
        ///   . a sequence of labels ending with a pointer.
        /// </remarks>
        public string GetDomain() {
            StringBuilder domain = new StringBuilder();
            int length = 0;

            while ((length = GetByte()) != 0) {
                // top 2 bits set denotes name compression and to reference elsewhere.
                if ((length & 0xc0) == 0xc0) {
                    int offset = ((length & 0x3f) << 8 | GetByte());
                    if (offset < 0 || offset > length_)
                        return null;

                    // save the position...
                    int position = position_;
                    // ...roll back the pointer position...
                    position_ = offset;
                    // ...to read the domain at given offset...
                    domain.Append(GetDomain());
                    // ...and restore the position.
                    position_ = position;

                    return domain.ToString();
                }

                while (length > 0) {
                    domain.Append(GetChar());
                    length--;
                }

                // if size of next label isn't null(end of domain name) add a period ready for next label.
                if (Peek() != 0)
                    domain.Append(".");
            }

            return domain.ToString();
        }
    }
}
