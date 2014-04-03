using System;
using System.Collections.Generic;

namespace Nohros.Bus
{
  public interface ICreateHandlers
  {
    IEnumerable<IHandle<T>> CreateHandlersOf<T>();
  }
}
