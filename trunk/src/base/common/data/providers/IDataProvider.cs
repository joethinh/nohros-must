using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    public interface IDataProvider
    {
        /// <summary>
        /// Gets the string used to open the connection.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the name of the owner of the database.
        /// </summary>
        string DatabaseOwner { get; }
    }
}