using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using Nohros.Data;
using Nohros.IO;

namespace Nohros.Data.SqlCe
{
  public class SqlCeAppState : IAppState
  {
    // Tables
    const string kBoolTableName = "bool_state";
    const string kShortTableName = "short_state";
    const string kIntTableName = "int_state";
    const string kLongTableName = "long_state";
    const string kDecimalTableName = "decimal_state";
    const string kDoubleTableName = "double_state";
    const string kStringTableName = "string_state";
    const string kGuidTableName = "guid_state";
    const string kDateTimeTableName = "date_state";

    readonly AddStateQuery add_state_;
    readonly GetStateQuery get_state_;
    readonly SetIfGreaterThanQuery if_greater_than_query_;
    readonly SetIfLessThanQuery if_less_than_query_;

    readonly SqlCeConnectionProvider sql_connection_provider_;

    readonly string[,] tables_ = new[,] {
      {kBoolTableName, "bit"},
      {kShortTableName, "smallint"},
      {kIntTableName, "int"},
      {kLongTableName, "bigint"},
      {kDecimalTableName, "decimal"},
      {kDoubleTableName, "float"},
      {kStringTableName, "nvarchar(1024)"},
      {kGuidTableName, "uniqueidentifier"},
      {kDateTimeTableName, "datetime"}
    };

    readonly UpdateStateQuery update_state_;

    bool supress_dtc_;

    public SqlCeAppState(SqlCeConnectionProvider sql_connection_provider,
      bool supress_dtc = true) {
      sql_connection_provider_ = sql_connection_provider;
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
    public void SetIfGreaterThan(string name, int state, int comparand) {
      SetIfGreater(name, state, comparand, kIntTableName);
    }

    /// <inheritdoc/>
    public void SetIfGreaterThan(string name, long state, long comparand) {
      SetIfGreater(name, state, comparand, kLongTableName);
    }

    /// <inheritdoc/>
    public void SetIfLessThan(string name, int state, int comparand) {
      SetIfLessThan(name, state, comparand, kIntTableName);
    }

    /// <inheritdoc/>
    public void SetIfLessThan(string name, long state, long comparand) {
      SetIfLessThan(name, state, comparand, kLongTableName);
    }

    /// <summary>
    /// Initialize the state repository.
    /// </summary>
    /// <remarks>
    /// The initialization process checks if the database referenced by the
    /// associated connection string exists and created a new one if not.
    /// </remarks>
    public void Initialize() {
      EnsureDatabase();
      var create_table = new CreateTableQuery(sql_connection_provider_);
      int j = tables_.GetLength(0);
      for (int i = 0; i < j; i++) {
        string name = tables_[i, 0];
        string type = tables_[i, 1];
        create_table.Execute(name, type);
      }
    }

    void EnsureDatabase() {
      var builder =
        new SqlCeConnectionStringBuilder(
          sql_connection_provider_.ConnectionString);

      if (!File.Exists(builder.DataSource)) {
        new SqlCeEngine(sql_connection_provider_.ConnectionString)
          .CreateDatabase();
      }
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

    void SetIfGreater(string name, long state, long comparand, string table_name) {
      // lets try to update the value upfront and if nothing changes,
      // check if the value exists and create a new one if not.
      if (!if_greater_than_query_.Execute(name, table_name, state, comparand)) {
        long obj;
        if (!get_state_.Execute(name, table_name, out obj)) {
          // If a insert is performed after our update attempt and before
          // our insert attempt a unique constraint exception will be throw.
          // In that case we will try to perform the update again.
          try {
            add_state_.Execute(name, table_name, state);
          } catch (UniqueConstraintViolationException) {
            if_greater_than_query_.Execute(name, table_name, state, comparand);
          }
        }
      }
    }

    void SetIfLessThan(string name, long state, long comparand,
      string table_name) {
      // lets try to update the value upfront and if nothing changes,
      // check if the value exists and create a new one if not.
      if (!if_less_than_query_.Execute(name, table_name, state, comparand)) {
        long obj;
        if (!get_state_.Execute(name, table_name, out obj)) {
          // If a insert is performed after our update attempt and before
          // our insert attempt a unique constraint exception will be throw.
          // In that case we will try to perform the update again.
          try {
            add_state_.Execute(name, table_name, state);
          } catch (UniqueConstraintViolationException) {
            if_less_than_query_.Execute(name, table_name, state, comparand);
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
