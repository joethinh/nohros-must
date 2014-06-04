using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace Nohros.Tests
{
  [TestFixture]
  public class GenericTest
  {
    string mSTRING =
      "ƒ‰¯~u–z''8¬kqf''K ²{u– qta''{q''x €…°{‡“¯lM''!is…¬#~‰ |q–}8A''x T…±ir…°mM''‹ms˜žz„‰°|u''";

    string mSENHA = "Punk";

    [Test]
    public string Desencrypta() {
      int[] numArray = new int[] {0x70, 0x5f, 0x7f, 0x4d};
      mSTRING = mSTRING.Trim().Replace("''", "'");
      string str2 = "";
      int num4 = mSTRING.Length - 1;
      for (int i = 0; i <= num4; i++) {
        if (Asc(mSTRING.Substring(i, 1)[0]) < 40) {
          str2 = str2 + mSTRING.Substring(i, 1);
        } else {
          int num3;
          int charCode = (Asc(mSTRING.Substring(i, 1)[0]) - 40) + 0xd8;
          if (mSENHA.Length > 0) {
            num3 = i%mSENHA.Length;
            charCode -= Asc(mSENHA.Substring(num3, 1)[0]);
          }
          num3 = i%numArray.Length;
          charCode -= numArray[num3];
          if (charCode < 40) {
            charCode += 0xd8;
          }
          if (charCode > 0x100) {
            charCode -= 0xd8;
          }
          str2 = str2 + (char)charCode;
        }
      }
      return str2;
    }

    public static int Asc(char String) {
      int num;
      int num2 = Convert.ToInt32(String);
      if (num2 < 0x80) {
        return num2;
      }
      try {
        byte[] buffer;
        Encoding fileIOEncoding = Encoding.Default;
        char[] chars = new char[] { String };
        if (fileIOEncoding.IsSingleByte) {
          buffer = new byte[1];
          int num3 = fileIOEncoding.GetBytes(chars, 0, 1, buffer, 0);
          return buffer[0];
        }
        buffer = new byte[2];
        if (fileIOEncoding.GetBytes(chars, 0, 1, buffer, 0) == 1) {
          return buffer[0];
        }
        if (BitConverter.IsLittleEndian) {
          byte num4 = buffer[0];
          buffer[0] = buffer[1];
          buffer[1] = num4;
        }
        num = BitConverter.ToInt16(buffer, 0);
      } catch (Exception exception) {
        throw exception;
      }
      return num;
    }
  }
}
