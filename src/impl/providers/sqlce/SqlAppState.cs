using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Diagnostics;
using Nohros.Data;

namespace Nohros.Data.SqlServer
{
  public class SqlAppState : IAppState
  {
    const string kClassName = "Nohros.Data.SqlServer.SqlAppState";
    const string kGetQueryBegin = @"select state from ";
    const string kGetQueryEnd = @" where name=@name";

    const string kCreateTableBegin = @"
if not exists(
  select name
  from sys.tables
  where name =";

    const string kCreateTableMiddle = @"
    ) create table ";
    const string kCreateTableEnd = @" (name varchar(512), state";

    const string kSetQueryBegin = @"
if exixts(
  select state from ";
    const string kSetQueryMiddle1 = @"
where name=@name);
begin
  insert into
";
    const string kSetQueryMiddle2 = @" (name, state) values(@name, @state);";
    const string kSetQueryMiddle3 = @"
else
begin
  update
";

    const string kSetQueryEnd = @"
set state = @state
where name = @name
";

    // Tables
    const string kBoolTableName = "bool_state";
    const string kShortTableName = "short_state";
    const string kIntTableName = "int_state";
    const string kLongTableName = "long_state";
    const string kDecimalTableName = "decimal_state";
    const string kFloatTableName = "float_state";
    const string kStringTableName = "string_state";
    const string kGuidTableName = "guid_state";
    const string kObjectTableName = "string_state";

    // Get state queries
    const string kGetBoolQuery =
      kGetQueryBegin
        + kBoolTableName
        + kGetQueryEnd;

    const string kGetShortQuery =
      kGetQueryBegin
        + kShortTableName
        + kGetQueryEnd;

    const string kGetIntQuery = kGetQueryBegin
      + kIntTableName
      + kGetQueryEnd;

    const string kGetLongQuery =
      kGetQueryBegin
        + kLongTableName
        + kGetQueryEnd;

    const string kGetDecimalQuery =
      kGetQueryBegin
        + kDecimalTableName
        + kGetQueryEnd;

    const string kGetFloatQuery =
      kGetQueryBegin
        + kFloatTableName
        + kGetQueryEnd;

    const string kGetStringQuery =
      kGetQueryBegin
        + kStringTableName
        + kGetQueryEnd;

    const string kGetGuidQuery =
      kGetQueryBegin
        + kGuidTableName
        + kGetQueryEnd;

    const string kGetObjectQuery =
      kGetQueryBegin
        + kObjectTableName
        + kGetQueryEnd;

    // Set state queries
    const string kSetBoolQuery =
      kSetQueryBegin
        + kBoolTableName
        + kSetQueryMiddle1
        + kBoolTableName
        + kSetQueryMiddle2
        + kBoolTableName
        + kSetQueryMiddle3
        + kBoolTableName
        + kSetQueryEnd;

    const string kSetShortQuery =
      kSetQueryBegin
        + kShortTableName
        + kSetQueryMiddle1
        + kShortTableName
        + kSetQueryMiddle2
        + kShortTableName
        + kSetQueryMiddle3
        + kShortTableName
        + kSetQueryEnd;

    const string kSetIntQuery =
      kSetQueryBegin
        + kIntTableName
        + kSetQueryMiddle1
        + kIntTableName
        + kSetQueryMiddle2
        + kIntTableName
        + kSetQueryMiddle3
        + kIntTableName
        + kSetQueryEnd;

    const string kSetLongQuery =
      kSetQueryBegin
        + kLongTableName
        + kSetQueryMiddle1
        + kLongTableName
        + kSetQueryMiddle2
        + kLongTableName
        + kSetQueryMiddle3
        + kLongTableName
        + kSetQueryEnd;

    const string kSetDecimalQuery =
      kSetQueryBegin
        + kDecimalTableName
        + kSetQueryMiddle1
        + kDecimalTableName
        + kSetQueryMiddle2
        + kDecimalTableName
        + kSetQueryMiddle3
        + kDecimalTableName
        + kSetQueryEnd;

    const string kSetFloatQuery =
      kSetQueryBegin
        + kFloatTableName
        + kSetQueryMiddle1
        + kFloatTableName
        + kSetQueryMiddle2
        + kFloatTableName
        + kSetQueryMiddle3
        + kFloatTableName
        + kSetQueryEnd;

    const string kSetStringQuery =
      kSetQueryBegin
        + kStringTableName
        + kSetQueryMiddle1
        + kStringTableName
        + kSetQueryMiddle2
        + kStringTableName
        + kSetQueryMiddle3
        + kStringTableName
        + kSetQueryEnd;

    const string kSetGuidQuery =
      kSetQueryBegin
        + kGuidTableName
        + kSetQueryMiddle1
        + kGuidTableName
        + kSetQueryMiddle2
        + kGuidTableName
        + kSetQueryMiddle3
        + kGuidTableName
        + kSetQueryEnd;

    const string kSetObjectQuery =
      kSetQueryBegin
        + kObjectTableName
        + kSetQueryMiddle1
        + kObjectTableName
        + kSetQueryMiddle2
        + kObjectTableName
        + kSetQueryMiddle3
        + kObjectTableName
        + kSetQueryEnd;

    // Create table queries
    const string kCreateBoolQuery =
      kCreateTableBegin
        + kBoolTableName
        + kCreateTableMiddle
        + kBoolTableName
        + kCreateTableEnd;

    const string kCreateShortQuery =
      kCreateTableBegin
        + kShortTableName
        + kCreateTableMiddle
        + kShortTableName
        + kCreateTableEnd;

    const string kCreateIntQuery =
      kCreateTableBegin
        + kIntTableName
        + kCreateTableMiddle
        + kIntTableName
        + kCreateTableEnd;

    const string kCreateLongQuery =
      kCreateTableBegin
        + kLongTableName
        + kCreateTableMiddle
        + kLongTableName
        + kCreateTableEnd;

    const string kCreateDecimalQuery =
      kCreateTableBegin
        + kDecimalTableName
        + kCreateTableMiddle
        + kDecimalTableName
        + kCreateTableEnd;

    const string kCreateFloatQuery =
      kCreateTableBegin
        + kFloatTableName
        + kCreateTableMiddle
        + kFloatTableName
        + kCreateTableEnd;

    const string kCreateStringQuery =
      kCreateTableBegin
        + kStringTableName
        + kCreateTableMiddle
        + kStringTableName
        + kCreateTableEnd;

    const string kCreateGuidQuery =
      kCreateTableBegin
        + kGuidTableName
        + kCreateTableMiddle
        + kGuidTableName
        + kCreateTableEnd;

    const string kCreateObjectQuery =
      kCreateTableBegin
        + kObjectTableName
        + kCreateTableMiddle
        + kObjectTableName
        + kCreateTableEnd;


    readonly SqlCeConnectionProvider sql_connection_provider_;

    public SqlAppState(SqlCeConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
    }

    public T Get<T>(string name) {
      T state;
      if (!Get(name, out state)) {
        throw new NoResultException();
      }
      return state;
    }

    public bool Get<T>(string name, out T state) {
      dynamic s;
      if (Get(name, out s)) {
        state = (T) s;
        return true;
      }
      state = default(T);
      return false;
    }

    public void Set<T>(string name, T state) {
    }

    /// <summary>
    /// Initialize the state repository.
    /// </summary>
    public void Initialize() {
      CreateTable(kCreateBoolQuery);
      CreateTable(kCreateDecimalQuery);
      CreateTable(kCreateFloatQuery);
      CreateTable(kCreateGuidQuery);
      CreateTable(kCreateIntQuery);
      CreateTable(kCreateLongQuery);
      CreateTable(kCreateObjectQuery);
      CreateTable(kCreateShortQuery);
      CreateTable(kCreateStringQuery);
    }

    void CreateTable(string query) {
      using (SqlCeConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(query)
          .SetType(CommandType.Text)
          .Build();
        try {
          conn.Open();
          cmd.ExecuteNonQuery();
        } catch (SqlException e) {
          throw new ProviderException(e);
        }
      }
    }

    [DebuggerStepThrough]
    public bool Get(string name, out bool state) {
      return Get(name, kGetBoolQuery, out state);
    }

    [DebuggerStepThrough]
    public bool Get(string name, out short state) {
      return Get(name, kGetShortQuery, out state);
    }

    [DebuggerStepThrough]
    public bool Get(string name, out int state) {
      return Get(name, kGetIntQuery, out state);
    }

    [DebuggerStepThrough]
    public bool Get(string name, out long state) {
      return Get(name, kGetLongQuery, out state);
    }

    [DebuggerStepThrough]
    public bool Get(string name, out decimal state) {
      return Get(name, kGetDecimalQuery, out state);
    }

    [DebuggerStepThrough]
    public bool Get(string name, out float state) {
      return Get(name, kGetFloatQuery, out state);
    }

    [DebuggerStepThrough]
    public bool Get(string name, out string state) {
      return Get(name, kGetStringQuery, out state);
    }

    [DebuggerStepThrough]
    public bool Get(string name, out Guid state) {
      return Get(name, kGetGuidQuery, out state);
    }

    [DebuggerStepThrough]
    public bool Get(string name, out object state) {
      return Get(name, kGetObjectQuery, out state);
    }

    [DebuggerStepThrough]
    public void Set(string name, bool state) {
      Set(name, kSetBoolQuery, state);
    }

    [DebuggerStepThrough]
    public void Set(string name, short state) {
      Set(name, kSetShortQuery, state);
    }

    [DebuggerStepThrough]
    public void Set(string name, int state) {
      Set(name, kSetIntQuery, state);
    }

    [DebuggerStepThrough]
    public void Set(string name, long state) {
      Set(name, kSetLongQuery, state);
    }

    [DebuggerStepThrough]
    public void Set(string name, decimal state) {
      Set(name, kSetDecimalQuery, state);
    }

    [DebuggerStepThrough]
    public void Set(string name, float state) {
      Set(name, kSetFloatQuery, state);
    }

    [DebuggerStepThrough]
    public void Set(string name, string state) {
      Set(name, kSetStringQuery, state);
    }

    [DebuggerStepThrough]
    public void Set(string name, Guid state) {
      Set(name, kSetGuidQuery, state);
    }

    [DebuggerStepThrough]
    public void Set(string name, object state) {
      Set(name, kSetObjectQuery, state);
    }

    void Set(string name, string query, object state) {
      using (SqlCeConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(query)
          .SetType(CommandType.StoredProcedure)
          .AddParameter("@name", name)
          .AddParameterWithValue("@state", state)
          .Build();
        try {
          conn.Open();
          cmd.ExecuteNonQuery();
        } catch (SqlException e) {
          throw new ProviderException(e);
        }
      }
    }

    bool Get<T>(string name, string query, out T state) {
      using (SqlCeConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(query)
          .SetType(CommandType.Text)
          .AddParameter("@name", name)
          .Build();
        try {
          conn.Open();
          object obj = cmd.ExecuteScalar();
          if (obj == null) {
            state = default(T);
            return false;
          }
          state = (T) obj;
          return true;
        } catch (SqlException e) {
          throw new ProviderException(e);
        }
      }
    }
  }
}
