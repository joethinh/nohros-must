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
    public class DataProvider<T> : IDataProvider where T : class, IDataProvider
    {
        /// <summary>
        /// The name of the data base owner.
        /// </summary>
        protected string databaseOwner_;

        /// <summary>
        /// The connection string used to open the database.
        /// </summary>
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
        protected DataProvider(string databaseOwner, string connectionString)
        {
            databaseOwner_ = databaseOwner;
            connectionString_ = connectionString;
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
        /// </remarks>
        /// <exception cref="ArgumentNullException">dataProvider is null</exception>
        /// <exception cref="ProviderException">The type could not be created.</exception>
        /// <exception cref="ProviderException"><paramref name="dataProvider"/> is invalid.</exception>
        public static T CreateInstance(Provider provider)
        {
            // Get the type.
            Type type = Type.GetType(provider.Type);

            T newObject = null;
            if (type != null) {
                newObject = (T)Activator.CreateInstance(type, new object[] { provider.DatabaseOwner, provider.ConnectionString });
            }

            // If a instance could not be created a exception will be thrown.
            if (newObject == null)
                Thrower.ThrowProviderException(ExceptionResource.DataProvider_CreateInstance, null);

            return newObject;
        }

        /// <summary>
        /// Creates an instance of the type designated by the specified generic type parameter using the
        /// constructor implied by the <see cref="IDataProvider"/> interface.
        /// </summary>
        /// <typeparam name="T">A type of a class that implements the IDataProvider interface.</typeparam>
        /// <param name="provider_name">The name of the data provider.</param>
        /// <param name="configuration">A class that implements the IConfiguration class containing the information
        /// about the application data provides.</param>
        /// <remarks>
        /// This method will try to get the data provider information from the <see cref="IConfiguration.GetProvider(string)"/>
        /// method using the specified provider name.
        /// </remarks>
        /// <returns>An instance of the type <typeparam name="T"/> if the provider information could be found and
        /// the class could be instantiated.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="provider_name"/> provider was not
        /// found in the <paramref name="configuration"/></exception>
        /// <exception cref="ThrowProviderException">The provider type is invalid or could not be instantiated.</exception>
        protected static T CreateInstance(string provider_name, IConfiguration configuration)
        {
            Provider provider = configuration.GetProvider(provider_name);
            if (provider == null)
                throw new ArgumentNullException(StringResources.DataProvider_InvalidProvider);
            return CreateInstance(provider);
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