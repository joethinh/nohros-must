using System;
using System.Data.SqlServerCe;

namespace Nohros.Data.SqlCe.Extensions
{
  public static class SqlCeExceptionExtensions
  {
    public static ProviderException AsProviderException(
      this SqlCeException exception) {
      switch (exception.NativeError) {
        case 25016: // duplicate index constraint
        case 25030: // duplicate check constraint
          return new UniqueConstraintViolationException(exception);
      }
      return new ProviderException(exception);
    }
  }
}
