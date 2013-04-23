using System;
using System.Data;

namespace Nohros.Data.Providers
{
  interface IEnlistedCommand
  {
    void Execute(IDbCommand command);
  }

  interface IEnlistedCommand<out T>
  {
    T Result { get; }
  }
}
