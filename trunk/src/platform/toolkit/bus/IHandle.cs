using System;

namespace Nohros.Bus
{
  public interface IHandle<T> where T : IMessage
  {
  }
}
