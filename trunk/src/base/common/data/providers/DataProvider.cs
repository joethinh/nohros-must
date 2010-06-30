using System;
using System.Configuration;
using System.Web;
using Microsoft.Win32;
using System.Text;

namespace Nohros.Data
{
    public class DataProvider
    {
        const string kDefaultDatabaseOwner = "dbo";

        /// <summary>
        /// Creates an instance of the type designated by the specified generic type parameter using the
        /// constructor implied by the <see cref="IDataProvider"/> interface.
        /// </summary>
        /// <param name="dataProvider">A <see cref="Provider"/> object containing the informations like
        /// connection string, database owner, etc; that will be used to creates the designated type</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <remarks>
        /// The <typeparamref name="T"/> parameter must be a class that implements the <see cref="IDataProvider"/>
        /// interface a class that derives from the <see cref="GenericDataProvider"/> class.
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
        public static T CreateInstance<T>(Provider dataProvider) where T : class, IDataProvider
        {
            // finding the current attributes
            string connectionString = null;
            string dbOwner = null;

            DataProvider.GetDataStoreParameters(dataProvider, out connectionString, out dbOwner);
            if (connectionString == null)
                Thrower.ThrowProviderException(ExceptionResource.DataProvider_InvalidProvider, null);

            // Get the type.
            Type type = Type.GetType(dataProvider.Type);
            
            T newObject = null;
            if (type != null) {
                newObject = (T)Activator.CreateInstance(type, new object[] { dbOwner, connectionString });
            }

            // If a instance could not be created a exception will be thrown.
            if (newObject == null)
                Thrower.ThrowProviderException(ExceptionResource.DataProvider_CreateInstance, null);

            return newObject;
        }

        /// <summary>
        /// Gets the default connection string.
        /// </summary>
        /// <history>
        ///     [neylor] - 2009-02-15 - Release
        /// </history>
        public static string GetConnectionString() {
            return GetConnectionString("SiteSqlServer", ConfigurationRepository.ConfigurationFile, null);
        }

        /// <summary>
        /// Gets the specified connection string.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string to return</param>
        /// <returns>The connection string</returns>
        /// <history>
        ///     [neylor] - 2009-02-15
        ///         Release
        /// </history>
        public static string GetConnectionString(string connectionStringName, ConfigurationRepository repository, string regkey)
        {
            switch (repository)
            {
                case ConfigurationRepository.ConfigurationFile:
                    ConnectionStringSettings strs = ConfigurationManager.ConnectionStrings[connectionStringName];
                    if(strs != null)
                        return strs.ConnectionString;

                    break;

                case ConfigurationRepository.WindowsRegistry:
                    if (string.IsNullOrEmpty(regkey))
                        regkey = "HKEY_LOCAL_MACHINE\\Software\\Nohros\\ConnectionStrings";

                    return (string)Registry.GetValue(regkey, connectionStringName, null);
            }
            return null;
        }

        /// <summary>
        /// Gets the default database owner.
        /// </summary>
        /// <history>
        ///     [neylor] - 2009-02-15
        ///         Release
        /// </history>
        public static string GetDatabaseOwner()
        {
            return GetDatabaseOwner("SiteSqlServerOwner", ConfigurationRepository.ConfigurationFile, null);
        }

        /// <summary>
        /// Gets the specified database owner.
        /// </summary>
        /// <param name="dbOwnerStringName">Name of the database owner string</param>
        /// <history>
        ///     [neylor] - 2009-02-15
        ///         Release
        /// </history>
        public static string GetDatabaseOwner(string dbOwnerStringName, ConfigurationRepository repository, string regkey)
        {
            switch (repository)
            {
                case ConfigurationRepository.ConfigurationFile:
                    ConnectionStringSettings strs = ConfigurationManager.ConnectionStrings[dbOwnerStringName];
                    if (strs != null)
                        return strs.ConnectionString;

                    break;

                case ConfigurationRepository.WindowsRegistry:
                    if(string.IsNullOrEmpty(regkey))
                        regkey = "HKEY_LOCAL_MACHINE\\Software\\Nohros\\ConnectionStrings";

                    return (string)Registry.GetValue(regkey, dbOwnerStringName, null);
            }
            return kDefaultDatabaseOwner;
        }

        /// <summary>
        /// Gets the connection string and database owner of the specified Provider
        /// </summary>
        /// <param name="dataProvider">Provider to get data</param>
        /// <param name="connectionString">The connection string of the Provider</param>
        /// <param name="dbOwner">The database owner of the Provider</param>
        /// <history>
        ///     [neylor] - 2009-02-16
        ///         Release
        /// </history>
        public static void GetDataStoreParameters(Provider dataProvider, out string connectionString, out string dbOwner)
        {
            ConfigurationRepository reposiroty = dataProvider.ConfigurationRepository;

            dbOwner = dataProvider.Attributes["databaseOwner"];
            if ((dbOwner == null) || (dbOwner.Trim().Length == 0))
            {
                dbOwner = dataProvider.Attributes["databaseOwnerStringName"];
                dbOwner = (dbOwner == null || dbOwner.Trim().Length == 0) ? kDefaultDatabaseOwner : GetDatabaseOwner(dbOwner, reposiroty, null);
            }
            
            connectionString = dataProvider.Attributes["connectionString"];
            if ((connectionString == null) || (connectionString.Trim().Length == 0))
            {
                connectionString = GetConnectionString(dataProvider.Attributes["connectionStringName"], reposiroty, null);

                // decrypt the connection string if it is encrypted
                if (dataProvider.IsEncrypted)
                    connectionString = NSecurity.BasicDeCryptoString(connectionString);
            }
        }
    }
}