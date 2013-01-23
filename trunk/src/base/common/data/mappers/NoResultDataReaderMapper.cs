using System;

namespace Nohros.Data
{
  /// <summary>
  /// A <see cref="DataReaderMapper{T}"/> that throws an
  /// <see cref="NoResultException"/> when the <see cref="Map"/> method is
  /// called.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  internal class NoResultDataReaderMapper<T> : DataReaderMapper<T>
  {
    static readonly NoResultDataReaderMapper<T> no_result_data_reader_mapper_;

    #region .ctor
    static NoResultDataReaderMapper() {
      no_result_data_reader_mapper_ = new NoResultDataReaderMapper<T>();
    }
    #endregion

    public override T Map() {
      throw new NoResultException();
    }

    public static NoResultDataReaderMapper<T> Default {
      get { return no_result_data_reader_mapper_; }
    }
  }
}
