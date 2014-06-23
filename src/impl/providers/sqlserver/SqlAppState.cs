using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Nohros.Data;
using Nohros.Data.SqlServer;
using Nohros.IO;

namespace Nohros.Data.SqlServer
{
  public class SqlAppState : IAppState
  {
    // Tables
    const string kBoolTableName = "nohros_state_bool";
    const string kShortTableName = "nohros_state_short";
    const string kIntTableName = "nohros_state_int";
    const string kLongTableName = "nohros_state_long";
    const string kDecimalTableName = "nohros_state_decimal";
    const string kDoubleTableName = "nohros_state_double";
    const string kStringTableName = "nohros_state_string";
    const string kGuidTableName = "nohros_state_guid";
    const string kDateTimeTableName = "nohros_state_date";

    readonly AddStateQuery add_state_;
    readonly GetStateQuery get_state_;
    readonly UpdateStateQuery update_state_;
    readonly SetIfGreaterThanQuery if_greater_than_query_;
    readonly SetIfLessThanQuery if_less_than_query_;

    bool supress_dtc_;

    public SqlAppState(SqlConnectionProvider sql_connection_provider,
      bool supress_dtc = false) {
      update_state_ = new UpdateStateQuery(sql_connection_provider);
      get_state_ = new GetStateQuery(sql_connection_provider);
      add_state_ = new AddStateQuery(sql_connection_provider);
      if_greater_than_query_ = new SetIfGreaterThanQuery(sql_connection_provider);
      if_less_than_query_ = new SetIfLessThanQuery(sql_connection_provider);
      SupressTransactions = supress_dtc;
    }

    /// <inheritdoc/>
    public T Get<T>(string name) {
      T state;
      if (!Get(name, out state)) {
        throw new NoResultException();
      }
      return state;
    }

    /// <inheritdoc/>
    public T Get<T>(string name, T def) {
      T state;
      if (!Get(name, out state)) {
        return def;
      }
      return state;
    }

    /// <inheritdoc/>
    public bool Get<T>(string name, out T state) {
      dynamic d = default(T);
      bool got;
      state = ExplicitGet(name, d, out got);
      return got;
    }

    /// <inheritdoc/>
    public void Set<T>(string name, T state) {
      ExplicitSet(name, (dynamic) state);
    }

    /// <inheritdoc/>
    public void SetIfGreaterThan(string name, int state) {
      SetIfGreater(name, state, kIntTableName);
    }

    /// <inheritdoc/>
    public void SetIfGreaterThan(string name, long state) {
      SetIfGreater(name, state, kLongTableName);
    }

    /// <inheritdoc/>
    public void SetIfLessThan(string name, int state) {
      SetIfLessThan(name, state, kIntTableName);
    }

    /// <inheritdoc/>
    public void SetIfLessThan(string name, long state) {
      SetIfLessThan(name, state, kLongTableName);
    }

    bool ExplicitGet(string name, bool m, out bool got) {
      bool state;
      got = get_state_.Execute(name, kBoolTableName, out state);
      return state;
    }

    short ExplicitGet(string name, short m, out bool got) {
      short state;
      got = get_state_.Execute(name, kShortTableName, out state);
      return state;
    }

    int ExplicitGet(string name, int m, out bool got) {
      int state;
      got = get_state_.Execute(name, kIntTableName, out state);
      return state;
    }

    long ExplicitGet(string name, long m, out bool got) {
      long state;
      got = get_state_.Execute(name, kLongTableName, out state);
      return state;
    }

    decimal ExplicitGet(string name, decimal m, out bool got) {
      decimal state;
      got = get_state_.Execute(name, kDecimalTableName, out state);
      return state;
    }

    double ExplicitGet(string name, double m, out bool got) {
      double state;
      got = get_state_.Execute(name, kDoubleTableName, out state);
      return state;
    }

    string ExplicitGet(string name, string m, out bool got) {
      string state;
      got = get_state_.Execute(name, kStringTableName, out state);
      return state;
    }

    Guid ExplicitGet(string name, Guid m, out bool got) {
      Guid state;
      got = get_state_.Execute(name, kGuidTableName, out state);
      return state;
    }

    DateTime ExplicitGet(string name, DateTime m, out bool got) {
      DateTime state;
      got = get_state_.Execute(name, kDateTimeTableName, out state);
      return state;
    }

    object ExplicitGet(string name, object m, out bool got) {
      object state;
      got = get_state_.Execute(name, kStringTableName, out state);
      return state;
    }

    void ExplicitSet(string name, bool state) {
      ExplicitSet(name, state, kBoolTableName);
    }

    void ExplicitSet(string name, short state) {
      ExplicitSet(name, state, kShortTableName);
    }

    void ExplicitSet(string name, int state) {
      ExplicitSet(name, state, kIntTableName);
    }

    void ExplicitSet(string name, long state) {
      ExplicitSet(name, state, kLongTableName);
    }

    void ExplicitSet(string name, decimal state) {
      ExplicitSet(name, state, kDecimalTableName);
    }

    void ExplicitSet(string name, double state) {
      ExplicitSet(name, state, kDoubleTableName);
    }

    void ExplicitSet(string name, string state) {
      ExplicitSet(name, state, kStringTableName);
    }

    void ExplicitSet(string name, Guid state) {
      ExplicitSet(name, state, kGuidTableName);
    }

    void ExplicitSet(string name, DateTime state) {
      ExplicitSet(name, state, kDateTimeTableName);
    }

    void ExplicitSet(string name, object state) {
      ExplicitSet(name, state, kStringTableName);
    }

    void ExplicitSet(string name, object state, string table_name) {
      // lets try to update the value upfront and if nothing changes,
      // try to add the value to the table.
      if (!update_state_.Execute(name, table_name, state)) {
        // If a insert is performed after our update attempt and before
        // our insert attempt a unique constraint exception will be throw.
        // In that case we will try to perform the update again.
        try {
          add_state_.Execute(name, table_name, state);
        } catch (UniqueConstraintViolationException) {
          update_state_.Execute(name, table_name, state);
        }
      }
    }

    void SetIfGreater(string name, long state, string table_name) {
      // lets try to update the value upfront and if nothing changes,
      // check if the value exists and create a new one if not.
      if (!if_greater_than_query_.Execute(name, table_name, state)) {
        long obj;
        if (!get_state_.Execute(name, table_name, out obj)) {
          // If a insert is performed after our update attempt and before
          // our insert attempt a unique constraint exception will be throw.
          // In that case we will try to perform the update again.
          try {
            add_state_.Execute(name, table_name, state);
          } catch (UniqueConstraintViolationException) {
            if_greater_than_query_.Execute(name, table_name, state);
          }
        }
      }
    }

    void SetIfLessThan(string name, long state, string table_name) {
      // lets try to update the value upfront and if nothing changes,
      // check if the value exists and create a new one if not.
      if (!if_less_than_query_.Execute(name, table_name, state)) {
        long obj;
        if (!get_state_.Execute(name, table_name, out obj)) {
          // If a insert is performed after our update attempt and before
          // our insert attempt a unique constraint exception will be throw.
          // In that case we will try to perform the update again.
          try {
            add_state_.Execute(name, table_name, state);
          } catch (UniqueConstraintViolationException) {
            if_less_than_query_.Execute(name, table_name, state);
          }
        }
      }
    }

    /// <summary>
    /// Gets a value indicating if transaction should be supressed.
    /// </summary>
    /// <remarks>
    /// SqlCe does not fully support distributed transactions. The engine
    /// allows only one connection to be enlisted in a transaction. The
    /// <see cref="SupressTransactions"/> property provides a convenient
    /// way to use more than one connection inside a single transaction by
    /// creating an isolated <see cref="TransactionScopes"/>.
    /// </remarks>
    public bool SupressTransactions {
      get { return supress_dtc_; }
      set {
        supress_dtc_ = value;
        update_state_.SupressTransactions = value;
        get_state_.SupressTransactions = value;
        add_state_.SupressTransactions = value;
      }
    }
  }
}
