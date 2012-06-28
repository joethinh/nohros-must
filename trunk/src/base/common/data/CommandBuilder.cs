using System;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// A class that builds instances of the <see cref="IDbCommand"/> class.
  /// </summary>
  public class CommandBuilder
  {
    readonly IDbConnection connection_;
    readonly IDbCommand command_;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandBuilder"/> class
    /// by using the specified <see cref="IDbConnection"/> object.
    /// </summary>
    /// <param name="connection">
    /// A <see cref="IDbConnection"/> object that should be associated with
    /// the builded command.
    /// </param>
    public CommandBuilder(IDbConnection connection) {
      connection_ = connection;
      command_ = connection.CreateCommand();
    }

    /// <summary>
    /// Sets the command text.
    /// </summary>
    /// <param name="text">
    /// The command text.
    /// </param>
    public CommandBuilder SetText(string text) {
      command_.CommandText = text;
      return this;
    }

    /// <summary>
    /// Sets the command type.
    /// </summary>
    /// <param name="type">
    /// The command type.
    /// </param>
    public CommandBuilder SetType(CommandType type) {
      command_.CommandType = type;
      return this;
    }

    /// <summary>
    /// Sets the command timeout.
    /// </summary>
    /// <param name="timeout">
    /// The command timeout.
    /// </param>
    public CommandBuilder SetTimeout(int timeout) {
      command_.CommandTimeout = timeout;
      return this;
    }

    /// <summary>
    /// Sets the associated transaction.
    /// </summary>
    /// <param name="transaction">
    /// The trasaction to associate with the command to built.
    /// </param>
    public CommandBuilder SetTransaction(IDbTransaction transaction) {
      command_.Transaction = transaction;
      return this;
    }

    /// <summary>
    /// Creates an <see cref="IDbDataParameter"/> object and add it to the
    /// collection of parameters of the <see cref="IDbCommand"/> that will
    /// be built.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter to add.
    /// </param>
    /// <param name="type">
    /// The type of the parameter to add.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="CommandBuilder"/> that will add the
    /// created parameter to the <see cref="IDbCommand"/> instance that will
    /// be created on <see cref="Build"/>.
    /// </returns>
    public CommandBuilder AddParameter(string name, DbType type) {
      CreateParameter(name, type);
      return this;
    }

    /// <summary>
    /// Creates an <see cref="IDbDataParameter"/> object and add it to the
    /// collection of parameters of the <see cref="IDbCommand"/> that will
    /// be built.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter to add.
    /// </param>
    /// <param name="type">
    /// The type of the parameter to add.
    /// </param>
    /// <param name="size">
    /// The size of the parameter.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="CommandBuilder"/> that will add the
    /// created parameter to the <see cref="IDbCommand"/> instance that will
    /// be created on <see cref="Build"/>.
    /// </returns>
    public CommandBuilder AddParameter(string name, DbType type, int size) {
      IDbDataParameter parameter = CreateParameter(name, type);
      parameter.Size = size;
      return this;
    }

    /// <summary>
    /// Creates an <see cref="IDbDataParameter"/> object and add it to the
    /// collection of parameters of the <see cref="IDbCommand"/> that will
    /// be built.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter to add.
    /// </param>
    /// <param name="type">
    /// The type of the parameter to add.
    /// </param>
    /// <param name="size">
    /// The size of the parameter.
    /// </param>
    /// <param name="value">
    /// The value to be associated with the parameter.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="CommandBuilder"/> that will add the
    /// created parameter to the <see cref="IDbCommand"/> instance that will
    /// be created on <see cref="Build"/>.
    /// </returns>
    public CommandBuilder AddParameter(string name, DbType type, int size, object value) {
      IDbDataParameter parameter = CreateParameter(name, type);
      parameter.Size = size;
      parameter.Value = value;
      return this;
    }

    /// <summary>
    /// Creates an <see cref="IDbDataParameter"/> object and add it to the
    /// collection of parameters of the <see cref="IDbCommand"/> that will
    /// be built.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter to add.
    /// </param>
    /// <param name="type">
    /// The type of the parameter to add.
    /// </param>
    /// <param name="value">
    /// The value to be associated with the parameter.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="CommandBuilder"/> that will add the
    /// created parameter to the <see cref="IDbCommand"/> instance that will
    /// be created on <see cref="Build"/>.
    /// </returns>
    public CommandBuilder AddParameter(string name, DbType type, object value) {
      IDbDataParameter parameter = CreateParameter(name, type);
      parameter.Value = value;
      return this;
    }

    /// <summary>
    /// Creates an <see cref="IDbDataParameter"/> object and add it to the
    /// collection of parameters of the <see cref="IDbCommand"/> that will
    /// be built.
    /// </summary>
    /// <param name="parameter">
    /// The parameter to be added.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="CommandBuilder"/> that will add the
    /// specified parameter to the <see cref="IDbCommand"/> instance that will
    /// be created on <see cref="Build"/>.
    /// </returns>
    public CommandBuilder AddParameter(IDbDataParameter parameter) {
      command_.Parameters.Add(parameter);
      return this;
    }

    IDbDataParameter CreateParameter(string name, DbType type) {
      IDbDataParameter parameter = command_.CreateParameter();
      parameter.ParameterName = name;
      parameter.DbType = type;
      command_.Parameters.Add(parameter);
      return parameter;
    }

    /// <summary>
    /// Creates an instance of the <see cref="IDbCommand"/> using the values
    /// configured through this builder.
    /// </summary>
    /// <returns>
    /// The newly created <see cref="IDbCommand"/> object.
    /// </returns>
    public IDbCommand Build() {
      return command_;
    }
  }
}
