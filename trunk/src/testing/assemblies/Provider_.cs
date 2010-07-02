using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NUnit.Framework;

using Nohros.Data;

namespace Nohros.Test
{
    public abstract class GenericDataProvider : DataProvider<GenericDataProvider>
    {
        const string kGenericProviderName = "GenericProvider";
        const string xml_node_repository =
            @"<xml>
                <providers>
                    <add
                        name=""GenericProvider""
                        type=""Nohros.Test.SqlGenericDataProvider, nohros.test.assemblies""
                        databaseOwner=""strings/mssql/@dbowner""
                        connectionString=""strings/mssql/@dbstring""
                        dataSourceType=""mssql""
                    />
                </providers>
                <strings>
                    <mssql dbowner=""dbo"" dbstring=""datasource=; usr=; pwd="" />
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
                        type=""Nohros.Test.ProviderType_, nohros.test.assemblies""
                        databaseOwner=""strings/mssql/@dbowner""
                        connectionString=""strings/mssql/@dbstring""
                        dataSourceType=""mssql""
                    />
                </providers>
                <strings>
                    <mssql dbowner=""dbo"" dbstring=""datasource=; usr=; pwd="" />
                </strings>
              </xml>";

        const string xml_node_no_connection =
            @"<xml>
                <providers>
                    <add
                        name=""MyName""
                        type=""Nohros.Test.ProviderType_, nohros.test.assemblies""
                        databaseOwner=""strings/mssql/@dbowner""
                        dataSourceType=""mssql""
                    />
                </providers>
                <strings>
                    <mssql dbowner=""dbo"" dbstring=""datasource=; usr=; pwd="" />
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
            Assert.AreEqual(provider.ConnectionString, "datasource=; usr=; pwd=");
            Assert.AreEqual(provider.DatabaseOwner, "dbo");
            Assert.AreEqual(provider.ConnectionStringsRepository, ConnectionStringsRepository.ConfigurationFile);
            Assert.AreEqual(provider.DataSourceType, DataSourceType.MsSql);
        }

        [Test]
        public void DataProvider_Instantiation()
        {
            GenericDataProvider data_provider_impl = GenericDataProvider.Instance;
            Assert.AreEqual(data_provider_impl.DatabaseOwner, "dbo");
            Assert.AreEqual(data_provider_impl.ConnectionString, "datasource=; usr=; pwd=");
            Assert.IsInstanceOf<SqlGenericDataProvider>(data_provider_impl);
        }
    }
}