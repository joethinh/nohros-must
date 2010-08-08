using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NUnit.Framework;
using System.Data.SqlClient;

using Nohros.Data;

namespace Nohros.Test.Data
{
    public abstract class GenericDataProvider : DataProvider<GenericDataProvider>
    {
        const string kGenericProviderName = "GenericProvider";
        const string xml_node_repository =
            @"<xml>
                <providers>
                    <add
                        name=""GenericProvider""
                        type=""Nohros.Test.Data.SqlGenericDataProvider, nohros.test.desktop""
                        databaseOwner=""xml/strings/mssql/@dbowner""
                        connectionString=""xml/strings/mssql/@dbstring""
                        dataSourceType=""mssql""
                    />
                </providers>
                <strings>
                    <mssql dbowner=""dbo"" dbstring=""Data Source='localhost';Initial Catalog='Hydra';User ID='nohros';Password='Noors03';Asynchronous Processing=true;"" />
                </strings>
              </xml>";

        static GenericDataProvider instance;

        static GenericDataProvider()
        {
            XmlDocument document = new XmlDocument();
            Provider provider;

            document.LoadXml(xml_node_repository);
            provider = new Provider(document.SelectSingleNode("//providers/add"));

            instance = CreateInstance(provider);
        }

        protected GenericDataProvider(string owner, string dbstring):base(owner, dbstring) {
        }

        public void QuerySomething() {
            Assert.DoesNotThrow(delegate() {
                using (SqlConnection conn = GetDbConnection<SqlConnection>()) {
                }
            });
        }

        public static GenericDataProvider Instance
        {
            get { return instance; }
        }
    }

    public class SqlGenericDataProvider : GenericDataProvider
    {
        public SqlGenericDataProvider(string dbowner, string dbstring) : base(dbowner, dbstring)
        {
        }
    }

    [TestFixture]
    public class Provider_
    {
        const string xml_node_repository =
            @"<xml>
                <providers>
                    <add
                        name=""GenericProvider""
                        type=""Nohros.Test.ProviderType_, nohros.test.desktop""
                        databaseOwner=""xml/strings/mssql/@dbowner""
                        connectionString=""xml/strings/mssql/@dbstring""
                        dataSourceType=""mssql""
                    />
                </providers>
                <strings>
                    <mssql dbowner=""dbo"" dbstring=""Data Source='localhost';Initial Catalog='Hydra';User ID='nohros';Password='Noors03';Asynchronous Processing=true;"" />
                </strings>
              </xml>";

        const string xml_node_no_connection =
            @"<xml>
                <providers>
                    <add
                        name=""MyName""
                        type=""Nohros.Test.ProviderType_, nohros.test.desktop""
                        databaseOwner=""strings/mssql/@dbowner""
                        dataSourceType=""mssql""
                        assemblyLocation=""/""
                    />
                </providers>
                <strings>
                    <mssql dbowner=""dbo"" dbstring=""Data Source='localhost';Initial Catalog='Hydra';User ID='nohros';Password='Noors03';Asynchronous Processing=true;"" />
                </strings>
              </xml>";

        [Test]
        public void Provider_ctor()
        {
            XmlDocument document = new XmlDocument();
            Provider provider;

            Assert.Throws<ProviderException>(delegate()
            {
                document.LoadXml(xml_node_no_connection);
                provider = new Provider(document.SelectSingleNode("//providers/add"));
            });
            Assert.Pass("Provider throws exception if a mandatory parameter is missing");

            document.LoadXml(xml_node_repository);
            provider = new Provider(document.SelectSingleNode("//providers/add"));
            Assert.AreEqual(provider.ConnectionString, "Data Source='localhost';Initial Catalog='Hydra';User ID='nohros';Password='Noors03';Asynchronous Processing=true;");
            Assert.AreEqual(provider.DatabaseOwner, "dbo");
            Assert.AreEqual(provider.ConnectionStringsRepository, ConnectionStringsRepository.ConfigurationFile);
            Assert.AreEqual(provider.DataSourceType, DataSourceType.MsSql);
        }

        [Test]
        public void DataProvider_Instantiation()
        {
            GenericDataProvider data_provider_impl = GenericDataProvider.Instance;
            Assert.AreEqual(data_provider_impl.DatabaseOwner, "dbo");
            Assert.AreEqual(data_provider_impl.ConnectionString, "Data Source='localhost';Initial Catalog='Hydra';User ID='nohros';Password='Noors03';Asynchronous Processing=true;");
            Assert.IsInstanceOf<SqlGenericDataProvider>(data_provider_impl);

            data_provider_impl.QuerySomething();
        }
    }
}