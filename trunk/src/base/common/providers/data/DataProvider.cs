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
using Nohros.Providers;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// An generic abstract implementation of the <see cref="IDataProvider"/>
  /// interface.
  /// </summary>
  public class DataProvider: IDataProvider
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
    /// Initializes a new instance of the <see cref="DataProvider"/> class.
    /// </summary>
    protected DataProvider() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataProvider"/> class
    /// by using the specified connection string and database owner.
    /// </summary>
    /// <param name="database_owner">The name of the database owner.</param>
    /// <param name="connection_string">A string tthat can be used to open a
    /// connection to the database.
    /// </param>
    protected DataProvider(string database_owner, string connection_string) {
      database_owner_ = database_owner;
      connection_string_ = connection_string;
    }

    /// <summary>
    /// Creates an instance of the type designated by the specified
    /// <paramref name="provider"/> object using a constructor with the
    /// following signature:
    ///   .ctor(string database_owner, string connection_string);
    /// </summary>
    /// <param name="provider">A <see cref="DataProviderNode"/> object that
    /// contains informations such the connection string, the database owner,
    /// etc; that will be used to creates the designated provider instance.
    /// </param>
    /// <returns>A reference to the newly created object.</returns>
    /// <remarks>
    /// <para>
    /// The connection string and database owner parameters passed to the
    /// constructor will be extracted from the specified
    /// <paramref name="provider"/> object.
    /// </para>
    /// <para>
    /// The type defined by the <paramref name="provider"/> object must have
    /// a constructor that accepts two strings as parameters. The first
    /// parameter will be set to the provider database owner and the second
    /// parameter will be set to the provider connection string.
    /// </para>
    /// <para>The <see cref="ProviderNode.AssemblyLocation"/> will be used as
    /// a search location for assemblies that need to be loaded in order to
    /// create the desired provider instance.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">provider is null.
    /// </exception>
    /// <exception cref="ProviderException">The type could not be created.
    /// </exception>
    protected static IDataProvider CreateDataProvider(
      DataProviderNode provider) {
      return CreateDataProvider(provider, provider.DatabaseOwner,
        provider.ConnectionString);
    }

    /// <summary>
    /// Creates an instance of the <see cref="IDataProvider"/> class by using
    /// the specified <paramref name="provider"/> object and a list of
    /// constructor arguments.
    /// </summary>
    /// <param name="provider">A <see cref="ProviderNode"/>
    /// object that contains information about the class that should be
    /// instantiated.</param>
    /// <param name="args">An array of arguments that match in number, order,
    /// and type the parameters of the constructor to invoke. If args is an
    /// empty array or null, the constructor that takes no parameters(the
    /// default constructor) is invoked.</param>
    /// <remarks>
    /// <para>The <see cref="ProviderNode.AssemblyLocation"/> will be used as
    /// a search location for assemblies that need to be loaded in order to
    /// create the desired provider instance.
    /// </para>
    /// </remarks>
    /// <returns>An object that implements the <see cref="IDataProvider"/>
    /// interface and has the same type that is defined by the
    /// <paramref name="provider"/> object.</returns>
    /// <exception cref="ArgumentNullException">provider is null.
    /// </exception>
    /// <exception cref="ProviderException">The type could not be created.
    /// </exception>
    protected static IDataProvider CreateDataProvider(
      DataProviderNode provider, params object[] args) {
      return ProviderHelper.CreateFromProviderNode<IDataProvider>(
        provider, args);
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
    public DataSourceType DataSourceType {
      get { return data_source_type_; }
      set { data_source_type_ = value; }
    }
  }
}