using System;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// Provides facilities methods for create data parameters.
  /// </summary>
  public sealed class DataParameters
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="IDbDataParameter"/> class
    /// that uses the parameter name and the data type.
    /// </summary>
    /// <param name="command">
    /// The <see cref="IDbCommand"/> object to which the created parameter
    /// should be associated.
    /// </param>
    /// <param name="parameter_name">
    /// The name of the parameter to map.
    /// </param>
    /// <param name="db_type">
    /// One of the <see cref="DbType"/> values that best represents the value
    /// of the parameter.
    /// </param>
    /// <returns>
    /// A instance of the <see cref="IDbDataParameter"/> class associated with
    /// the given <see cref="IDbCommand"/> object.
    /// </returns>
    public static IDbDataParameter CreateParameter(IDbCommand command,
      string parameter_name, DbType db_type) {
      IDbDataParameter parameter = command.CreateParameter();
      parameter.ParameterName = parameter_name;
      parameter.DbType = db_type;
      return parameter;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IDbDataParameter"/> class
    /// that uses the parameter name and the data type.
    /// </summary>
    /// <param name="command">
    /// The <see cref="IDbCommand"/> object to which the created parameter
    /// should be associated.
    /// </param>
    /// <param name="parameter_name">
    /// The name of the parameter to map.
    /// </param>
    /// <param name="db_type">
    /// One of the <see cref="DbType"/> values that best represents the value
    /// of the parameter.
    /// </param>
    /// <param name="size">
    /// The length of the parameter.
    /// </param>
    /// <returns>
    /// A instance of the <see cref="IDbDataParameter"/> class associated with
    /// the given <see cref="IDbCommand"/> object.
    /// </returns>
    public static IDbDataParameter CreateParameter(IDbCommand command,
      string parameter_name, DbType db_type, int size) {
      IDbDataParameter parameter = CreateParameter(command, parameter_name,
        db_type);
      parameter.Size = size;
      return parameter;
    }
  }
}
