using System;
using System.Collections.Generic;
using Nohros.Resources;

namespace Nohros.Metrics
{
  /// <summary>
  /// A metric name with the ability to include semantic tags.
  /// </summary>
  public class MetricName
  {
    public MetricName(string key) : this(key, new Dictionary<string, string>()) {
    }

    public MetricName(string key, IDictionary<string, string> tags) {
      Key = key;
      Tags = tags;
    }

    public string Key { get; set; }

    public IDictionary<string, string> Tags { get; private set; }
  }
}
