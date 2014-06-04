using System;
using System.Collections.Generic;
using NUnit.Framework;
using Nohros.Configuration;
using Nohros.Configuration.Builders;
using Nohros.Providers;

namespace Nohros.Extensions
{
  public class Configurations
  {
    public class Provider
    {
    }

    public class ProviderFactory : IProviderFactory<Provider>
    {
      object IProviderFactory.CreateProvider(IDictionary<string, string> options) {
        return CreateProvider(options);
      }

      public Provider CreateProvider(IDictionary<string, string> options) {
        return new Provider();
      }
    }

    [Test]
    public void should_create_an_instance_of_all_registered_provider_factories() {
      var config = new Configuration.Configuration();
      config.Providers.Add("my-provider", typeof(ProviderFactory));

      Assert.That(() => config.CreateProviders(o => {}), Throws.Nothing);
      var objects = config.CreateProviders();
      Assert.That(objects.Length, Is.EqualTo(1));
    }

    [Test]
    public void should_created_an_instance_only_if_provider_implements_IProviderFactory_interface() {
      var config = new Configuration.Configuration();
      config.Providers.Add("my-provider", typeof(ProviderFactory));
      config.Providers.Add("my-provider-2", typeof (string));

      Assert.That(() => config.CreateProviders(o => { }), Throws.Nothing);
      var objects = config.CreateProviders();
      Assert.That(objects.Length, Is.EqualTo(1));
    }
  }
}
