using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funq;

namespace Nohros.ServiceStack
{
  public static class ContainerExtensions
  {
    public static Container Register(this Container container, Type type,
      object obj) {
      var mi = container.GetType()
                        .GetMethods()
                        .First(x => x.Name == "Register"
                          && x.GetParameters().Length == 1
                          && x.ReturnType == typeof (void))
                        .MakeGenericMethod(type);
      mi.Invoke(container, new[] {obj});
      return container;
    }

    public static Container Register(this Container container, object obj,
      params Type[] type) {
      foreach (var t in type) {
        container.Register(t, obj);
      }
      return container;
    }
  }
}
