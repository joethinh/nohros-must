using System;

namespace Nohros.Bus
{
  public interface IHandle<in T>
  {
    void Handle(T msg);
  }
}
