using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Nohros.Net
{
    public sealed class Utility
    {
        private static NetSettings settings_ = null;

		internal static string MapPath(string path)
		{
			string ppath;

			HttpContext ctx = HttpContext.Current;
			if (ctx != null)
				ppath = HttpContext.Current.Server.MapPath(path);
			else
			{
				string root = AppDomain.CurrentDomain.BaseDirectory;
				if (path.StartsWith("~/"))
					ppath = root + path.Substring(2);
				else
					ppath = root + path;
			}
			return ppath;
        }

        /// <summary>
        /// Gets the base directory that the assembly resolver used to probe assemblies
        /// </summary>
        internal static string BaseDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        #region Web utility
        /// <summary>
        /// Metodo de validação/limpeza de dados vindos do usuário
        /// </summary>
        /// <param name="text">Entrada do usuario</param>
        /// <param name="maxLength">Tamanho máximo da string</param>
        /// <returns>Versão limpa do texto de entrada</returns>
        public static string InputText(string text, int maxLength)
        {
            text.Trim();
            if (maxLength == 0)
                maxLength = 1;
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "[\\s]{2,}", " ");   // dois os mais espaços
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");   // <br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");   //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);    // outras tags
            text = Regex.Replace(text, "'", "''");
            return text;
        }
        public static string HostPath(Uri uri)
        {
            string str = (uri.Port == 80) ? string.Empty : (":" + uri.Port.ToString());
            return uri.Scheme + "://" + uri.Host + str;
        }
        public static string JSQuote(string str)
        {
            return string.Concat("\"", str, "\"");
        }
        #endregion
    }
}
