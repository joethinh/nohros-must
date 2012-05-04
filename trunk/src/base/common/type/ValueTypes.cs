using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros
{
  public class ValueTypes
  {
    /// <summary>
    /// Returns a ValueType with the specified <paramref name="ValueType"/> and
    /// whose value is equivalent to the specified <paramref name="s"/>
    /// </summary>
    /// <param name="type">
    /// A <paramref name="ValueType"/>
    /// </param>
    /// <param name="s">
    /// A string containing a value to parse.
    /// </param>
    /// <param name="result">
    /// When this method returns, a ValueType whose type is
    /// <paramref name="ValueType"/> an whose value is equivalent to
    /// <paramref name="s"/> or default(ValueType) if the conversion is not
    /// supported.
    /// </param>
    /// <returns></returns>
    public static bool TryParse(Type type, string s, out System.ValueType result) {
      result = default(System.ValueType);

      if (!type.IsValueType)
        return false;

      bool ret;

      if (type == typeof(int)) {
        int i;
        ret = int.TryParse(s, out i);
        result = i;
        return ret;
      }
      if (type == typeof(bool)) {
        bool boo;
        ret = bool.TryParse(s, out boo);
        result = boo;
        return ret;
      }
      if (type == typeof(decimal)) {
        decimal decima;
        ret = decimal.TryParse(s, out decima);
        result = decima;
        return ret;
      }
      if (type == typeof(DateTime)) {
        DateTime datetime;
        ret = DateTime.TryParse(s, out datetime);
        result = datetime;
        return ret;
      }
      if (type == typeof(long)) {
        long lon;
        ret = long.TryParse(s, out lon);
        result = lon;
        return ret;
      }
      if (type == typeof(float)) {
        float floa;
        ret = float.TryParse(s, out floa);
        result = floa;
        return ret;
      }
      if (type == typeof(double)) {
        double doubl;
        ret = double.TryParse(s, out doubl);
        result = doubl;
        return ret;
      }
      if (type == typeof(char)) {
        char cha;
        ret = char.TryParse(s, out cha);
        result = cha;
        return ret;
      }
      if (type == typeof(uint)) {
        uint uin;
        ret = uint.TryParse(s, out uin);
        result = uin;
        return ret;
      }
      if (type == typeof(byte)) {
        byte byt;
        ret = byte.TryParse(s, out byt);
        result = byt;
        return ret;
      }
      if (type == typeof(short)) {
        short shor;
        ret = short.TryParse(s, out shor);
        result = shor;
        return ret;
      }
      if (type == typeof(ulong)) {
        ulong ulon;
        ret = ulong.TryParse(s, out ulon);
        result = ulon;
        return ret;
      }
      if (type == typeof(ushort)) {
        ushort ushor;
        ret = ushort.TryParse(s, out ushor);
        result = ushor;
        return ret;
      }
      if (type == typeof(sbyte)) {
        sbyte sbyt;
        ret = sbyte.TryParse(s, out sbyt);
        result = sbyt;
        return ret;
      }
      return false;
    }
  }
}