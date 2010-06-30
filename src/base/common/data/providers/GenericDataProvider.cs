using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Nohros.Resources;
using Nohros.Configuration;

namespace Nohros.Data
{
    /// <summary>
    /// An generic abstract implementation of the IDataProvider interface
    /// </summary>
    public class GenericDataProvider : IDataProvider
    {
        protected string databaseOwner_;
        protected string connectionString_;

        /// <summary>
        /// Initializes a new instance of the GenericDataProvider by using the specified
        /// connection string and database owner.
        /// </summary>
        /// <param name="databaseOwner">The name of the database owner.</param>
        /// <param name="connectionString">A string used to open a database.</param>
        /// <remarks>
        /// This constructor is called by the DataProvider.CreateInstance&lt;T&gt; method.
        /// </remarks>
        protected GenericDataProvider(string databaseOwner, string connectionString)
        {
            databaseOwner_ = databaseOwner;
            connectionString_ = connectionString;
        }


        protected static T CreateDataProviderInstance<T>(string provider_name, IConfiguration configuration) where T : class, IDataProvider
        {
            Provider provider = configuration.GetProvider(provider_name);
            if (provider == null)
                throw new ArgumentNullException(StringResources.DataProvider_InvalidProvider);
            return DataProvider.CreateInstance<T>(provider);
        }

        /// <summary>
        /// Gets the string used to open the connection.
        /// </summary>
        public string ConnectionString {
            get { return databaseOwner_; }
        }

        /// <summary>
        /// Gets the name of the owner of the database.
        /// </summary>
        public string DatabaseOwner {
            get { return connectionString_; }
        }
   }
}