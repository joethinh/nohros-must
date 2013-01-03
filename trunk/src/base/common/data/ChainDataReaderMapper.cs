using System;
using System.Data;

namespace Nohros.Data
{
  public class ChainDataReaderMapper<T, T1> : DataReaderMapper<T>,
                                              IChainMapper<T, T1>
    where T1 : IMapper<T1>
  {
    readonly DataReaderMapper<T> mapper_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ChainDataReaderMapper{T,T1}"/> class by using the
    /// specified <see cref="IDataReader"/> and
    /// <see cref="DataReaderMapper{T}"/> objects.
    /// </summary>
    public ChainDataReaderMapper(DataReaderMapper<T> mapper) {
      mapper_ = mapper;
    }
    #endregion

    /// <inheritdoc/>
    public override T Map() {
      return mapper_.Map();
    }

    /// <summary>
    /// Gets the <see cref="DataReaderMapper{T}"/> that can be used to map the
    /// next record set.
    /// </summary>
    /// <remarks>
    /// When <see cref="DataReaderMapper{T}"/> is called the associated
    /// <see cref="IDataReader"/> is advanced to the next record set.
    /// </remarks>
    public bool NextResult(out IMapper<T1> t) {
      if (mapper_.reader_.NextResult()) {
        t = new DataReaderMapper<T1>
          .Builder()
          .Build(mapper_.reader_);
        return true;
      }
      t = default(T1);
      return false;
    }
  }
}
