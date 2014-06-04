using System;
using Nohros.Configuration;

namespace Nohros.RestQL
{
  public interface ISettings : IConfiguration
  {
    string QueryServerAddress { get; }
    int ResponseTimeout { get; }
  }
}
