using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Win32;
using System.IO;

namespace Nohros.Data
{
    #region enumerations
    public enum DataSourceType
    {
        MsSql = 0,
        OleDb = 1,
        Odbc = 2,
        Unknown = 100
    }
    #endregion

    /// <summary>
    /// Stores all the data needed to create instances of the IDataProvider provider's implementation
    /// of the data source classes.
    /// </summary>
    public class Provider
    {
        string name_;
        string type_;
        string database_owner_;
        string connection_string_;
        string assembly_location_;

        DataSourceType data_source_;
        NameValueCollection attributes_;

        #region .ctor
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Provider()
        {
            name_ = null;
            type_ = null;
            attributes_ = new NameValueCollection();
            data_source_ = DataSourceType.Unknown;
            database_owner_ = "dbo";
            connection_string_ = null;
            assembly_location_ = null;
        }
        #endregion

        /// <summary>
        /// Gets the attributes of the Provider
        /// </summary>
        public NameValueCollection Attributes
        {
            get { return attributes_; }
            set { attributes_ = value; }
        }

        /// <summary>
        /// Gets the name of the Provider.
        /// </summary>
        public string Name
        {
            get { return name_; }
            set { name_ = value; }
        }

        /// <summary>
        /// Gets the type of the Provider.
        /// </summary>
        public string Type
        {
            get { return type_; }
            set { type_ = value; }
        }

        /// <summary>
        /// Gets the type of the data source that will be used by the provider.
        /// </summary>
        public DataSourceType DataSourceType
        {
            get { return data_source_; }
            set { data_source_ = value; }
        }

        /// <summary>
        /// Gets the name of the owner of the database
        /// </summary>
        public string DatabaseOwner {
            get { return database_owner_; }
            set { database_owner_ = value; }
        }

        /// <summary>
        /// Gets a connection string taht can be used to open the database.
        /// </summary>
        public string ConnectionString {
            get { return connection_string_; }
            set { connection_string_ = value; }
        }

        /// <summary>
        /// Gets a string representing the fully qualified path to the directory where
        /// the assembly related with the provider is located.
        /// </summary>
        /// <remarks>
        /// This must be an absolute path or a path relative to the configuration file.
        /// </remarks>
        public string AssemblyLocation {
            get { return assembly_location_; }
            set { assembly_location_ = value; }
        }
    }
}