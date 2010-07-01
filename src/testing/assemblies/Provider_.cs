using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NUnit.Framework;

using Nohros.Data;

namespace Nohros.Test
{
    [TestFixture]
    public class Provider_
    {
        const string xml_node_repository =
            @"<xml>
                <providers>
                    <add
                        name=""MyName""
                        type=""Nohros.Test.ProviderType_, nohros.test.assemblies""
                        databaseOwner=""strings/mssql/@dbowner""
                        connectionString=""strings/mssql/@dbstring""
                    />
                </providers>
                <strings>
                    <mssql dbowner=""dbo"" dbstring=""datasource=; usr=; pwd="" />
                </strings>
              </xml>";

        [Test]
        public void Provider_NoException()
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml_node_repository);

            Provider provider = new Provider(document.SelectSingleNode("//providers/add"));
            Assert.AreEqual(provider.ConnectionString, "datasource=; usr=; pwd=");
            Assert.AreEqual(provider.DatabaseOwner, "dbo");
            Assert.AreEqual(provider.ConnectionStringsRepository, ConnectionStringsRepository.ConfigurationFile);
            Assert.AreEqual(provider.DataSourceType, DataSourceType.MsSql);
        }
    }
}