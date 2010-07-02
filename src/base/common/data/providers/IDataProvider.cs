using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Nohros.Data
{
    /// <summary>
    /// Allows an object to implements a DataProvider, and represents a set of methods and properties used
    /// to query database.
    /// </summary>
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

        /// <summary>
        /// Gets the data source type. ex. mssql, oracle, oledb...
        /// </summary>
        DataSourceType DataSourceType { get; set; }
    }
}