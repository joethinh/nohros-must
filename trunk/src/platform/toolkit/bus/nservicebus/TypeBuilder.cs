using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NServiceBus;
using NServiceBus.ObjectBuilder;

namespace Nohros.Bus.NServiceBus
{
  internal class TypeBuilder : ICreateHandlers, IConfigureTypes
  {
    readonly IBuilder builder_;

    readonly IConfigureComponents configurer_;

    public TypeBuilder(IBuilder builder, IConfigureComponents configurer) {
      builder_ = builder;
      configurer_ = configurer;
    }

    public ITypeConfig ConfigureType(Type type, Lifecycle lifecycle) {
      return new TypeConfig(
        configurer_.ConfigureComponent(type, GetDependencyLifecycle(lifecycle)));
    }

    public ITypeConfig<T> ConfigureType<T>(Lifecycle lifecycle) {
      return new TypeConfig<T>(
        configurer_.ConfigureComponent<T>(GetDependencyLifecycle(lifecycle)));
    }

    public ITypeConfig<T> ConfigureType<T>(Func<T> factory, Lifecycle lifecycle) {
      return new TypeConfig<T>(
        configurer_.ConfigureComponent(factory,
          GetDependencyLifecycle(lifecycle)));
    }

    public IConfigureTypes ConfigureProperty<T>(
      Expression<Func<T, object>> property, object value) {
      configurer_.ConfigureProperty(property, value);
      return this;
    }

    public IConfigureTypes ConfigureProperty<T>(string property, object value) {
      configurer_.ConfigureProperty<T>(property, value);
      return this;
    }

    public IConfigureTypes RegisterSingleton(Type type, object instance) {
      configurer_.RegisterSingleton(type, instance);
      return this;
    }

    public IConfigureTypes RegisterSingleton<T>(object instance) {
      configurer_.RegisterSingleton<T>(instance);
      return this;
    }

    public bool HasType<T>() {
      return configurer_.HasComponent<T>();
    }

    public bool HasType(Type type) {
      return configurer_.HasComponent(type);
    }

    public IEnumerable<IHandle<T>> CreateHandlersForType<T>() {
      return builder_.BuildAll<IHandle<T>>();
    }

    DependencyLifecycle GetDependencyLifecycle(Lifecycle lifecycle) {
      switch (lifecycle) {
        case Lifecycle.InstancePerCall:
          return DependencyLifecycle.InstancePerCall;

        case Lifecycle.SingleInstance:
          return DependencyLifecycle.SingleInstance;

        default:
          throw new ArgumentOutOfRangeException("lifecycle");
      }
    }
  }
}
