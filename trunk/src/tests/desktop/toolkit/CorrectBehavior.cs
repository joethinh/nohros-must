using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using NUnit.Framework;

using Nohros.Toolkit.DnsLookup;

namespace Nohros.Test.Toolkit.DnsLookup
{
    /// <summary>
    /// Summary description for CorrectBehaviour.
    /// </summary>
    [TestFixture]
    public class CorrectBehaviour
    {
        public CorrectBehaviour() {
        }

        [Test]
        public void CorrectMXForCodeProject() {
            // also 194.72.0.114
            MXRecord[] records = Resolver.MXLookup("codeproject.com", IPAddress.Parse("194.74.65.68"));

            Assert.IsNotNull(records, "MXLookup returning null denoting lookup failure");
            Assert.AreEqual(1, records.Length);
            Assert.AreEqual("mail.codeproject.com", records[0].DomainName);
            Assert.AreEqual(10, records[0].Preference);
        }

        [Test]
        [ExpectedException(typeof(NoResponseException))]
        public void NoResponseForBadDnsAddress() {
            MXRecord[] records = Resolver.MXLookup("codeproject.com", IPAddress.Parse("84.234.16.185"));
        }
    }
}
