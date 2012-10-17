using System;

namespace Nohros.Toolkit.RestQL
{
  public interface ISettings
  {
    string QueryServerAddress { get; }
    int ResponseTimeout { get; }
  }
}
