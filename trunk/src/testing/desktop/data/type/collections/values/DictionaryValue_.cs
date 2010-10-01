using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Data;
using Nohros.Configuration;

namespace Nohros.Test.Data.Type
{
    [TestFixture]
    public class DictionaryValue_
    {
        static DictionaryValue FillDictionary() {
            DictionaryValue dict = new DictionaryValue();
            CommonNode common = new CommonNode();
            dict.Add("common.providers.commondataprovider", new DataProviderNode("commondataprovider", "System.String"));
            dict.Add("common.providers.testdataprovider", new DataProviderNode("testdataprovider", "System.String"));
            dict.Add("common.providers.nohrosdataprovider", new DataProviderNode("nohrosdataprovider", "System.String"));
            dict.Add("common.providers.userdataprovider", new DataProviderNode("userdataprovider", "System.String"));
            return dict;
        }

        [Test]
        public void Add() {
            DictionaryValue dict = new DictionaryValue();
            CommonNode common = new CommonNode();
            dict.Add("common.providers.commondataprovider", new DataProviderNode("commondataprovider", "System.String"));
            Assert.Pass();
        }

        [Test]
        public void Get() {
            DictionaryValue dict = FillDictionary();
            DictionaryValue dict2 = dict["common.providers"] as DictionaryValue;
            Assert.IsNotNull(dict2);
            Assert.AreEqual(4, dict2.Size);
        }

        [Test]
        public void ToArray() {
            DictionaryValue dict = FillDictionary()["common.providers"] as DictionaryValue;
            ProviderNode[] nodes = dict.ToArray<ProviderNode>();
            Assert.IsNotNull(nodes);
            Assert.AreEqual(4, nodes.Length);
            Assert.AreEqual("commondataprovider", nodes[0].Name);
            Assert.AreEqual("testdataprovider", nodes[1].Name);
            Assert.AreEqual("nohrosdataprovider", nodes[2].Name);
            Assert.AreEqual("userdataprovider", nodes[3].Name);
        }
    }
}
