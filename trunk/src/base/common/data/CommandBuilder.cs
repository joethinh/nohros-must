﻿using System;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// A helper class used to simplify the instantiantion of objects of the type
  /// <see cref="IDbCommand"/>.
  /// </summary>
  /// <remarks>
  /// The command object is created at constructor and its properties
  /// manipulated through this class methods. Calling any of the class
  /// methods will change the properties of the returned object, even after
  /// <see cref="Build"/> is called.
  /// <para>
  /// This class implements the <see cref="IDisposable"/> only to allow
  /// callers to dispose the associated <see cref="IDbCommand"/> object. Do not
  /// dispose <see cref="CommandBuilder"/> object if you want to reuse the
  /// associated <see cref="IDbCommand"/> after class destruction.
  /// </para>
  /// </remarks>
  public class CommandBuilder : IDisposable
  {
    readonly IDbCommand command_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandBuilder"/> class
    /// by using the specified <see cref="IDbConnection"/> object.
    /// </summary>
    /// <param name="connection">
    /// A <see cref="IDbConnection"/> object that should be associated with
    /// the builded command.
    /// </param>
    public CommandBuilder(IDbConnection connection) {
      command_ = connection.CreateCommand();
    }
    #endregion

    /// <summary>
    /// Disposes the associated command object.
    /// </summary>
    public void Dispose() {
      command_.Dispose();
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
      IDbDataParameter parameter;
      return AddParameter(name, type, out parameter);
    }

    /// <summary>
    /// Creates an <see cref="IDbDataParameter"/> object and add it to the
    /// collection of parameters of the <see cref="IDbCommand"/> that will
    /// be built.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter to add.
    /// </param>
    /// <param name="value">
    /// The value of the parameter to add.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="CommandBuilder"/> that will add the
    /// created parameter to the <see cref="IDbCommand"/> instance that will
    /// be created on <see cref="Build"/>.
    /// </returns>
    public CommandBuilder AddParameterWithValue(string name, object value) {
      IDbDataParameter parameter;
      return AddParameter(name, value, out parameter);
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
      IDbDataParameter parameter;
      return AddParameter(name, type, size, out parameter);
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
    public CommandBuilder AddParameter(string name, object value, DbType type,
      int size) {
      IDbDataParameter parameter;
      return AddParameter(name, value, type, size, out parameter);
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
    public CommandBuilder AddParameter(string name, object value, DbType type) {
      IDbDataParameter parameter;
      return AddParameter(name, value, type, out parameter);
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
    public CommandBuilder AddParameter(string name, DbType type,
      out IDbDataParameter parameter) {
      parameter = CreateParameter(name, type);
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
    /// <param name="value">
    /// The value of the parameter to add.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="CommandBuilder"/> that will add the
    /// created parameter to the <see cref="IDbCommand"/> instance that will
    /// be created on <see cref="Build"/>.
    /// </returns>
    public CommandBuilder AddParameter(string name, object value,
      out IDbDataParameter parameter) {
      parameter = CreateParameterWithValue(name, value);
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
    public CommandBuilder AddParameter(string name, DbType type, int size,
      out IDbDataParameter parameter) {
      parameter = CreateParameter(name, type, size);
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
    public CommandBuilder AddParameter(string name, object value, DbType type,
      int size, out IDbDataParameter parameter) {
      parameter = CreateParameter(name, value, type, size);
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
    public CommandBuilder AddParameter(string name, object value, DbType type,
      out IDbDataParameter parameter) {
      parameter = CreateParameter(name, value, type);
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
    /// The newly created <see cref="IDbDataParameter"/>.
    /// </returns>
    public IDbDataParameter CreateParameter(string name, DbType type) {
      IDbDataParameter parameter = CreateParameter(name);
      parameter.DbType = type;
      return parameter;
    }

    /// <summary>
    /// Creates an <see cref="IDbDataParameter"/> object and add it to the
    /// collection of parameters of the <see cref="IDbCommand"/> that will
    /// be built.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter to add.
    /// </param>
    /// <param name="value">
    /// The value to be associated with the parameter.
    /// </param>
    /// <returns>
    /// The newly created <see cref="IDbDataParameter"/>.
    /// </returns>
    public IDbDataParameter CreateParameterWithValue(string name, object value) {
      IDbDataParameter parameter = CreateParameter(name);
      parameter.Value = value;
      return parameter;
    }

    /// <summary>
    /// Creates an <see cref="IDbDataParameter"/> object and add it to the
    /// collection of parameters of the <see cref="IDbCommand"/> that will
    /// be built.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter to add.
    /// </param>
    /// <returns>
    /// The newly created <see cref="IDbDataParameter"/>.
    /// </returns>
    public IDbDataParameter CreateParameter(string name) {
      IDbDataParameter parameter = command_.CreateParameter();
      parameter.ParameterName = name;
      command_.Parameters.Add(parameter);
      return parameter;
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
    /// The newly created <see cref="IDbDataParameter"/>.
    /// </returns>
    public IDbDataParameter CreateParameter(string name, DbType type, int size) {
      IDbDataParameter parameter = CreateParameter(name, type);
      parameter.Size = size;
      return parameter;
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
    /// The newly created <see cref="IDbDataParameter"/>.
    /// </returns>
    public IDbDataParameter CreateParameter(string name, object value,
      DbType type, int size) {
      IDbDataParameter parameter = CreateParameter(name, type, size);
      parameter.Value = value;
      return parameter;
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
    /// The newly created <see cref="IDbDataParameter"/>.
    /// </returns>
    public IDbDataParameter CreateParameter(string name, object value,
      DbType type) {
      IDbDataParameter parameter = CreateParameter(name, type);
      parameter.Value = value;
      return parameter;
    }

    /// <summary>
    /// Creates an instance of the <see cref="IDbCommand"/> using the values
    /// configured through this builder.
    /// </summary>
    /// <returns>
    /// The newly created <see cref="IDbCommand"/> object.
    /// </returns>
    /// <remarks>
    /// The command object is created at constructor and its properties
    /// manipulated through this class methods. Calling any of the class
    /// methods will change the properties of the returned object, even after
    /// <see cref="Build"/> is called.
    /// </remarks>
    public IDbCommand Build() {
      return command_;
    }
  }
}