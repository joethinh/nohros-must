using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    public abstract class DataTransferObject : IDataTransferObject
    {
        public static string GetJsElement(params string[] items)
        {
            int j = items.Length;

            if (j > 8) {
                string ret = string.Empty;
                for (int i = 0; i < j; i++) {
                    ret += "'" + items[i] + "',";
                }
                return "[" + ret.Substring(0, ret.Length - 1) + "]";
            }
            else {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < j; i++) {
                    sb.Append("'");
                    sb.Append(items[i]);
                    sb.Append("',");
                }
                return "[" + sb.ToString(0, sb.Length - 1) + "]";
            }
        }

        protected string ToJsElement(params string[] items)
        {
            return DataTransferObject.GetJsElement(items);
        }

        public abstract string ToJsElement();
        public abstract string ToJsObject();
    }
}
