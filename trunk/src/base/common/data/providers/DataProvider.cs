using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Reflection;
using System.IO;

using Nohros.Resources;
using Nohros.Configuration;

namespace Nohros.Data.Providers
{
    /// <summary>
    /// An generic abstract implementation of the IDataProvider interface
    /// </summary>
    public class DataProvider<T> : IDataProvider where T : class, IDataProvider
    {
        /// <summary>
        /// The name of the data base owner.
        /// </summary>
        protected string database_owner_;

        /// <summary>
        /// The connection string used to open the database.
        /// </summary>
        protected string connection_string_;

        /// <summary>
        /// The type of the underlying data source provider.
        /// </summary>
        protected DataSourceType data_source_type_;

        /// <summary>
        /// Initializes a new instance_ of the GenericDataProvider by using the specified
        /// connection string and database owner.
        /// </summary>
        /// <param name="databaseOwner">The name of the database owner.</param>
        /// <param name="connectionString">A string used to open a database.</param>
        /// <remarks>
        /// This constructor is called by the DataProvider.CreateInstance&lt;T&gt; method.
        /// </remarks>
        protected DataProvider(string databaseOwner, string connectionString)
        {
            database_owner_ = databaseOwner;
            connection_string_ = connectionString;
        }

        /// <summary>
        /// Creates an instance of the type designated by the specified generic type parameter using the
        /// constructor implied by the <see cref="IDataProvider"/> interface.
        /// </summary>
        /// <param name="provider">A <see cref="Provider"/> object containing the informations like
        /// connection string, database owner, etc; that will be used to creates the designated type</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <remarks>
        /// The <typeparamref name="T"/> parameter must be a class that implements or derive from a class
        /// that implements the <see cref="IDataProvider"/> interface.
        /// <para>
        /// The connection string and database owner parameters passed to the IDataProvider constructor will
        /// be extracted from the specified dataProvider object.
        /// </para>
        /// <para>
        /// The type T must have a constructor that accepts two strings as parameters. The first parameter
        /// will be set to the provider database owner and the second parameter will be set to the
        /// provider connection string.
        /// </para>
        /// <para>If the <see cref="ProviderNode.AssemblyLocation"/> of the specified provider is null this method will try
        /// to load assembly related with the provider type from the application base directory.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">dataProvider is null</exception>
        /// <exception cref="ProviderException">The type could not be created.</exception>
        /// <exception cref="ProviderException"><paramref name="dataProvider"/> is invalid.</exception>
        protected static T CreateInstance(DataProviderNode provider)
        {
            Type type = ProviderHelper.GetTypeFromProviderNode(provider);

            T newObject = null;
            if (type != null) {
                newObject = (T)Activator.CreateInstance(type, provider.DatabaseOwner, provider.ConnectionString);
            }

            // if a instance could not be created a exception will be thrown.
            if (newObject == null)
                throw new ProviderException(string.Format(StringResources.Type_CreateInstanceOf, "IDataProvider"));

            newObject.DataSourceType = provider.DataSourceType;

            return newObject;
        }

        /// <summary>
        /// Creates an instance_ of the type designated by the specified generic type parameter using the
        /// constructor implied by the <see cref="IDataProvider"/> interface.
        /// </summary>
        /// <typeparam name="T">A type of a class that implements the IDataProvider interface.</typeparam>
        /// <param name="provider_name">The name of the data provider.</param>
        /// <remarks>
        /// This method will try to get the data provider information from the <see cref="IConfiguration.GetProvider(string)"/>
        /// method using the specified provider name.
        /// <para>
        /// The type T must have a constructor that accepts two strings as parameters. The first parameter
        /// will be set to the provider database owner and the second parameter will be set to the
        /// provider connection string.
        /// </para>
        /// </remarks>
        /// <returns>An instance_ of the type <typeparam name="T"/> if the provider information could be found and
        /// the class could be instantiated.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="provider_name"/> provider was not
        /// found in the <paramref name="configuration"/></exception>
        /// <exception cref="Nohros.Data.ProviderException">The provider type is invalid or could not be instantiated.</exception>
        protected static T CreateInstance(string provider_name, NohrosConfiguration config)
        {
            if (config == null)
                throw new ProviderException(StringResources.Config_InvalidObject);

            CommonNode common = config.CommonNode;
            if (common == null)
                throw new ProviderException(StringResources.Config_InvalidObject);

            DataProviderNode provider = config.DataProviderNodes[provider_name] as DataProviderNode;
            if (provider == null)
                throw new ProviderException(StringResources.Config_InvalidObject);

            return CreateInstance(provider);
        }

        /// <summary>
        /// Gets an IDbConnection instance_ that can be used to query a data source.
        /// </summary>
        /// <returns>An IDbConnection instance_ that can be used to query a data source.</returns>
        /// <remarks>
        /// The returned type depends on the value of the <see cref="DataSourceType"/> property.
        /// </remarks>
        protected U GetDbConnection<U>() where U : class, IDbConnection
        {
            try
            {
                switch (data_source_type_)
                {
                    case DataSourceType.MsSql:
                        return new SqlConnection(connection_string_) as U;
                    case DataSourceType.Odbc:
                        return new OdbcConnection(connection_string_) as U;
                    case DataSourceType.OleDb:
                        return new OleDbConnection(connection_string_) as U;
                }
            }
            catch(Exception e) {
                throw new ProviderException(StringResources.DataProvider_Connection, e);
            }
            throw new ProviderException(StringResources.DataProvider_Connection);
        }

        /// <summary>
        /// Gets the string used to open the connection.
        /// </summary>
        public string ConnectionString {
            get { return connection_string_; }
        }

        /// <summary>
        /// Gets the name of the owner of the database.
        /// </summary>
        public string DatabaseOwner {
            get { return database_owner_; }
        }

        /// <summary>
        /// Gets the type of the underlying data source provider.
        /// </summary>
        public DataSourceType DataSourceType
        {
            get { return data_source_type_; }
            set { data_source_type_ = value; }
        }
   }
}