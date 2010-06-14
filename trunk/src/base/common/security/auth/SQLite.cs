using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Nohros.Security.Auth
{
    public sealed class SQLite
    {
        const int BUFFER_SIZE = 0x200;
        internal static string path = string.Empty;

        /// <summary>
        /// Ensure that the SQLite database file exist in the application base directory.
        /// </summary>
        /// <remarks>
        /// This method checks if the SQLite database file exist in the application base directory.
        /// If the file does not exists, this method creates a new one.
        /// </remarks>
        public static void EnsureSQLite()
        {
            int count = 0;
            byte[] buffer = new byte[BUFFER_SIZE];
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(dir, "nas.sqlite");

            if (!File.Exists(path))
            {
                // If the SQLite database file does not exists copy
                // the embedded database file from Assembly to disk.
                Assembly nas = Assembly.GetExecutingAssembly();
                using (Stream stream = nas.GetManifestResourceStream("Nohros.nas.sqlite"))
                {
                    using(Stream output = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, BUFFER_SIZE, false))
                    {
                        while ((count = stream.Read(buffer, 0, BUFFER_SIZE)) > 0)
                            output.Write(buffer, 0, count);
                    }
                }
            }
        }
    }
}
