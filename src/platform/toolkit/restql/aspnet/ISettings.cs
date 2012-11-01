using System;

namespace Nohros.RestQL
{
  public interface ISettings
  {
    string QueryServerAddress { get; }
    int ResponseTimeout { get; }
  }
}
