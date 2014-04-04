using System;
using System.Linq.Expressions;
using NServiceBus.ObjectBuilder;

namespace Nohros.Bus.NServiceBus
{
  internal class TypeConfig : ITypeConfig
  {
    readonly IComponentConfig config_;

    public TypeConfig(IComponentConfig config) {
      config_ = config;
    }

    ITypeConfig ITypeConfig.ConfigureProperty(string name,
      object value) {
      config_.ConfigureProperty(name, value);

      return this;
    }
  }

  internal class TypeConfig<T> : ITypeConfig<T>
  {
    readonly IComponentConfig<T> config_;

    public TypeConfig(IComponentConfig<T> config) {
      config_ = config;
    }

    ITypeConfig<T> ITypeConfig<T>.ConfigureProperty(
      Expression<Func<T, object>> property, object value) {
      config_.ConfigureProperty(property, value);

      return this;
    }
  }
}
