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
      object obj;
      if (get_state_.Execute(name, kBoolTableName, out obj)) {
        update_state_.Execute(name, kBoolTableName, state);
      } else {
        add_state_.Execute(name, kBoolTableName, state);
      }
    }

    void ExplicitSet(string name, short state) {
      object obj;
      if (get_state_.Execute(name, kShortTableName, out obj)) {
        update_state_.Execute(name, kShortTableName, state);
      } else {
        add_state_.Execute(name, kShortTableName, state);
      }
    }

    void ExplicitSet(string name, int state) {
      object obj;
      if (get_state_.Execute(name, kIntTableName, out obj)) {
        update_state_.Execute(name, kIntTableName, state);
      } else {
        add_state_.Execute(name, kIntTableName, state);
      }
    }

    void ExplicitSet(string name, long state) {
      object obj;
      if (get_state_.Execute(name, kLongTableName, out obj)) {
        update_state_.Execute(name, kLongTableName, state);
      } else {
        add_state_.Execute(name, kLongTableName, state);
      }
    }

    void ExplicitSet(string name, decimal state) {
      object obj;
      if (get_state_.Execute(name, kDecimalTableName, out obj)) {
        update_state_.Execute(name, kDecimalTableName, state);
      } else {
        add_state_.Execute(name, kDecimalTableName, state);
      }
    }

    void ExplicitSet(string name, double state) {
      object obj;
      if (get_state_.Execute(name, kDoubleTableName, out obj)) {
        update_state_.Execute(name, kDoubleTableName, state);
      } else {
        add_state_.Execute(name, kDoubleTableName, state);
      }
    }

    void ExplicitSet(string name, string state) {
      object obj;
      if (get_state_.Execute(name, kStringTableName, out obj)) {
        update_state_.Execute(name, kStringTableName, state);
      } else {
        add_state_.Execute(name, kStringTableName, state);
      }
    }

    void ExplicitSet(string name, Guid state) {
      object obj;
      if (get_state_.Execute(name, kGuidTableName, out obj)) {
        update_state_.Execute(name, kGuidTableName, state);
      } else {
        add_state_.Execute(name, kGuidTableName, state);
      }
    }

    void ExplicitSet(string name, DateTime state) {
      object obj;
      if (get_state_.Execute(name, kDateTimeTableName, out obj)) {
        update_state_.Execute(name, kDateTimeTableName, state);
      } else {
        add_state_.Execute(name, kDateTimeTableName, state);
      }
    }

    void ExplicitSet(string name, object state) {
      object obj;
      if (get_state_.Execute(name, kStringTableName, out obj)) {
        update_state_.Execute(name, kStringTableName, state);
      } else {
        add_state_.Execute(name, kStringTableName, state);
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
