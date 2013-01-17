using System;

namespace Nohros.Data
{
  public interface IChainMapper<T, T1> : IMapper<T> where T1 : IMapper<T1>
  {
    /// <summary>
    /// Gets the mapper that should be used to map the next record set.
    /// </summary>
    bool NextResult(out IMapper<T1> mapper);
  }
}
