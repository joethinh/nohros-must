using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Nohros.Collections;
using Nohros.Dynamics;

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
#if DEBUG
  public abstract partial class DataReaderMapper<T> : IDataReaderMapper<T>
#else
  internal abstract class DataReaderMapper<T> : IDataReaderMapper<T>
#endif
  {
    class Enumerator : IEnumerable<T>
    {
      readonly DataReaderMapper<T> mapper_;
      readonly Action<T> post_map_;
      readonly IDataReader reader_;

      #region .ctor
      public Enumerator(IDataReader reader, DataReaderMapper<T> mapper,
        Action<T> post_map) {
        mapper_ = mapper;
        reader_ = reader;
        post_map_ = post_map;
      }
      #endregion

      IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
      }

      public IEnumerator<T> GetEnumerator() {
        T t;
        while (mapper_.Map(reader_, out t)) {
          post_map_(t);
          yield return t;
        }
      }
    }

    internal CallableDelegate<T> loader_;

    #region .ctor
    internal DataReaderMapper() {
      loader_ = NewT;
    }
    #endregion

#if DEBUG
    public void Save(string assembly_file_name) {
      Dynamics_.AssemblyBuilder.Save(assembly_file_name);
    }
#endif
    internal abstract T MapInternal(IDataReader reader);

    protected CallableDelegate<T> Loader {
      get { return loader_; }
      set { loader_ = value; }
    }

    bool Map(IDataReader reader, out T t) {
      if (reader.Read()) {
        t = MapInternal(reader);
        return true;
      }
      t = default(T);
      return false;
    }

    public T Map(IDataReader reader) {
      T t;
      GetOrdinals(reader);
      if (!Map(reader, out t)) {
        throw new NoResultException();
      }
      return t;
    }

    public T Map(IDataReader reader, Action<T> post_map) {
      T t = Map(reader);
      post_map(t);
      return t;
    }

    public IEnumerable<T> Map(IDataReader reader, bool defer) {
      return Map(reader, defer, delegate { });
    }

    public IEnumerable<T> Map(IDataReader reader, bool defer, Action<T> post_map) {
      GetOrdinals(reader);
      var enumerable = new Enumerator(reader, this, post_map);
      if (defer) {
        return enumerable;
      }
      return new List<T>(enumerable);
    }

    internal virtual void GetOrdinals(IDataReader reader) {
      throw new NotImplementedException();
    }

    internal virtual T NewT() {
      throw new NotImplementedException();
    }
  }
}
