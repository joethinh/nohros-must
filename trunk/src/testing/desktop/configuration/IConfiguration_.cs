using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Configuration;

namespace Nohros.Test.Configuration
{
    [TestFixture]
    public class IConfiguration_ : IConfiguration
    {
        public IConfiguration_():base("providers", "properties")
        {
        }

        [Test]
        public void IConfiguration_Load() {
            this.Load("sample.config", "/sample");
        }
    }
}
