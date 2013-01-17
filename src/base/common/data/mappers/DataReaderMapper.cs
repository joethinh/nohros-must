using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// An implementation of the <see cref="IMapper{T}"/> interface that uses
  /// a <see cref="IDataReader"/> to fetch the data to be mapped.
  /// </summary>
  /// <remarks>
  /// This class implements only <see cref="IEnumerable{T}"/> interface.
  ///<para>
  /// Each time the <see cref="IEnumerator.MoveNext"/> method of the
  /// returned <see cref="IEnumerator{T}"/> object is called, the associated
  /// <see cref="IDataReader"/> is advanced to the next record.
  /// </para>
  /// </remarks>
  public abstract partial class DataReaderMapper<T> : IMapper<T>, IDisposable
  {
    protected internal IDataReader reader_;

    #region .ctor
    protected DataReaderMapper() {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataReaderMapper{T}"/>
    /// using the specified <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> containing the data to be mapped.
    /// </param>
    protected DataReaderMapper(IDataReader reader) {
      reader_ = reader;
    }
    #endregion

    public void Dispose() {
      reader_.Dispose();
    }

    public abstract T Map();

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator() {
      while (reader_.Read()) {
        yield return Map();
      }
      reader_.Close();
    }
  }
}
