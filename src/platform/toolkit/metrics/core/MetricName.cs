using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A value class encapsulating a metric's owning class and name.
  /// </summary>
  /// <remarks>
  /// This class provides a unique name for a metric, which consist of four
  /// pieces of information.
  /// <list type="bullet">
  /// <item>group - The top level grouping of the metric. When a metric
  /// belongs to a class, this is default to the class's namespace.(e.g.,
  /// nohros.toolkit.metrics).
  /// </item>
  /// <item>type - The second level grouping of the metric. When a metric
  /// belongs to a class, this is default to the class's name(e.g.,
  /// MetricName).</item>
  /// <item>name - A short name describing the metric's purpose(e.g.,
  /// session-count).</item>
  /// <item>scope - An optional name describing the metric's scope. Useful for
  /// when you have multiple instances of a class.</item>
  /// </list>
  /// </remarks>
  public class MetricName
  {
    string group_;
    string type_;
    string name_;
    string scope_;
    string metric_name_as_string_;

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricName"/> without a
    /// scope.
    /// </summary>
    /// <param name="klass">The type of the class which
    /// <see cref="IMetric"/> belongs.</param>
    /// <param name="name">A short name describing the metric's purpose(e.g.,
    /// session-count).</param>
    /// <exception cref="ArgumentNullException">klass or name is a null
    /// reference.</exception>
    public MetricName(Type klass, string name)
      : this(klass, name, string.Empty) { }

    /// <summary>
    /// Creates a new <see cref="MetricName"/> without using the specified
    /// metric class, name and scope.
    /// </summary>
    /// <param name="klass">The type of the class which
    /// <see cref="IMetric"/> belongs.</param>
    /// <param name="name">A short name describing the metric's purpose(e.g.,
    /// session-count).</param>
    /// <param name="scope">An string describing the metric's scope. Useful for
    /// when you have multiple instances of a class.</param>
    /// <exception cref="ArgumentNullException">klass, name or scope is a
    /// null reference.</exception>
    public MetricName(Type klass, string name, string scope) :
      this((klass == null) ? null : klass.Namespace, name, scope) { }

    /// <summary>
    /// Creates a new <see cref="MetricName"/> without a scope using the
    /// specified metric group, type and name.
    /// </summary>
    /// <param name="group">The top level grouping of the metric. When a metric
    /// belongs to a class, this is default to the class's namespace.(e.g.,
    /// nohros.toolkit.metrics).</param>
    /// <param name="type">The second level grouping of the metric. When a
    /// metric belongs to a class, this is default to the class's name(e.g.,
    /// MetricName).</param>
    /// <param name="name">A short name describing the metric's purpose(e.g.,
    /// session-count).</param>
    /// <exception cref="ArgumentNullException">group, type or name is a
    /// null reference.</exception>
    public MetricName(string group, string type, string name)
      : this(group, type, name, string.Empty) { }

    /// <summary>
    /// Creates a new <see cref="MetricName"/>.
    /// </summary>
    /// <param name="group">The top level grouping of the metric. When a metric
    /// belongs to a class, this is default to the class's namespace.(e.g.,
    /// nohros.toolkit.metrics).</param>
    /// <param name="type">The second level grouping of the metric. When a
    /// metric belongs to a class, this is default to the class's name(e.g.,
    /// MetricName).</param>
    /// <param name="name">A short name describing the metric's purpose(e.g.,
    /// session-count).</param>
    /// <param name="scope">An string describing the metric's scope. Useful for
    /// when you have multiple instances of a class.</param>
    /// <exception cref="ArgumentNullException">group, type, name or scope
    /// is a null reference.</exception>
    public MetricName(string group, string type, string name, string scope) {
      if (group == null || type == null || name == null || scope == null) {
        throw new ArgumentNullException(
          group == null ? "group" :
            type == null ? "type" :
              scope == null ? "scope" : "name");
      }
      group_ = group;
      type_ = type;
      name_ = name;
      scope_ = scope;
      metric_name_as_string_ = MetricNameAsString(group, type, name, scope);
    }

    /// <summary>
    /// Builds a string that uniquely represents the metric within an
    /// application.
    /// </summary>
    /// <param name="group">The top level grouping of the metric. When a metric
    /// belongs to a class, this is default to the class's namespace.(e.g.,
    /// nohros.toolkit.metrics).</param>
    /// <param name="type">The second level grouping of the metric. When a
    /// metric belongs to a class, this is default to the class's name(e.g.,
    /// MetricName).</param>
    /// <param name="name">A short name describing the metric's purpose(e.g.,
    /// session-count).</param>
    /// <param name="scope">An string describing the metric's scope. Useful for
    /// when you have multiple instances of a class.</param>
    /// <returns></returns>
    static string MetricNameAsString(string group, string type,
      string name, string scope) {

      string metricname_as_string = string.Concat(group, ":type=", type);
      if (scope.Length > 0) {
        metricname_as_string += ",scope=" + scope;
      }

      if (name.Length > 0) {
        metricname_as_string += ",name=";
        metricname_as_string += name;
      }
      return metricname_as_string;
    }

    /// <summary>
    /// Gets the group which the <see cref="IMetric"/> belongs. For class-based
    /// metrics, this will be the namespace of the class to which the
    /// <see cref="IMetric"/> belongs.
    /// </summary>
    /// <value>The group which the <see cref="IMetric"/> belongs.</value>
    public string Group {
      get { return group_; }
    }

    /// <summary>
    /// Gets the type which the <see cref="IMetric"/> belongs. For class-based
    /// metrics, this will be the simple class name of the class to which the
    /// <see cref="IMetric"/> belongs.
    /// </summary>
    /// <value>The type which the <see cref="IMetric"/> belongs.</value>
    public string Type {
      get { return type_; }
    }

    /// <summary>
    /// Gets the name of the <see cref="IMetric"/>.
    /// </summary>
    /// <value>The name of the <see cref="IMetric"/>.</value>
    public string Name {
      get { return name_; }
    }

    /// <summary>
    /// Gets the scope of the <see cref="IMetric"/>.
    /// </summary>
    /// <value>The scope of the <see cref="IMetric"/> or a empty string if
    /// the scope was not specified.</value>
    /// <remarks>
    /// If the scope was not defined a empty string will be returned.
    /// </remarks>
    public string Scope {
      get { return scope_; }
    }

    /// <summary>
    /// Gets a value indicating if the scope was defined or not.
    /// </summary>
    /// <value><c>true</c> if the scope was defined on the constructor;
    /// othwerwise, false.</value>
    /// <remarks>A scope is considered defined when it is not equals to a
    /// empty string. If a empty string is defined as the scope on the
    /// constructor, this method will returns, false.</remarks>
    public bool HasScope {
      get { return (scope_ != string.Empty); }
    }

    /// <summary>
    /// Gets the string representation of the <see cref="IMetric"/> identified
    /// by this metric name.
    /// </summary>
    /// <returns>The string representation of the <see cref="IMetric"/>
    /// identified by this metric.</returns>
    /// <remarks>The value returned by this method is unique across an
    /// application and should be used in equality comparison.</remarks>
    public override string ToString() {
      return metric_name_as_string_;
    }

    /// <summary>
    /// Determines whether the specified <see cref="Object"/> is equals to
    /// the current <see cref="MetricName"/> object.
    /// </summary>
    /// <param name="obj">The <see cref="Object"/> to compare with the current
    /// <see cref="MetricName"/> object.</param>
    /// <returns><c>true</c> if the specified <see cref="Object"/> is equals
    /// to the current <see cref="MetricName"/> object; otherwise, <c>false</c>
    /// </returns>
    public override bool Equals(object obj) {
      MetricName other = obj as MetricName;
      if (other == null) {
        return false;
      }
      return other.metric_name_as_string_ == this.metric_name_as_string_;
    }

    /// <summary>
    /// Determines whether the specified <see cref="MetricNaname"/> is equals
    /// to the current <see cref="MetricName"/> object.
    /// </summary>
    /// <param name="metric_name">The <see cref="MetricNaname"/> to compare
    /// with the current <see cref="MetricName"/> object.</param>
    /// <returns><c>true</c> if the specified <see cref="MetricNaname"/> is
    /// equals to the current <see cref="MetricName"/> object; otherwise,
    /// <c>false</c>.
    /// </returns>
    public bool Equals(MetricName metric_name) {
      if ((object)metric_name == null) {
        return false;
      }
      return this.metric_name_as_string_ == metric_name.metric_name_as_string_;
    }

    /// <summary>
    /// Servers as a hash function for the <see cref="MetricName"/> type.
    /// </summary>
    /// <returns>A hash code for the current <see cref="MetricName"/> object.
    /// </returns>
    public override int GetHashCode() {
      return metric_name_as_string_.GetHashCode();
    }
  }
}
