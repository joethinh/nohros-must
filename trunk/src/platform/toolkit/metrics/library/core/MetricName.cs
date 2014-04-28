using System;
using System.Collections.Generic;

namespace Nohros.Metrics
{
  /// <summary>
  /// A metric name with the ability to include semantic tags.
  /// </summary>
  /// <remarks>
  /// Tags allow the natural grouping of similar metrics, wich give users the
  /// ability to ealisy correlate freely between the metrics that incorporate
  /// them.
  /// 
  /// For example, take the following two metrics
  /// 
  ///  * nohros.com.interface.traffic.eth0.in
  ///  * nohros.com.interface.traffic.eth0.out
  /// 
  /// Tags simplifies that representation, these would be represented as:
  /// 
  /// * interface.traffic {host=nohros.com, interface=eth0, direction=in}
  /// * interface.traffic {host=nohros.com, interface=eth0, direction=out}
  /// </remarks>
  public class MetricName
  {
    readonly IDictionary<string, string> tags_;
    readonly int hashcode_;

    public MetricName(string key) : this(key, new Dictionary<string, string>()) {
    }

    public MetricName(string key, IDictionary<string, string> tags) {
      tags_ = tags;
      Key = key;
      hashcode_ = ComputeHashCode();
    }

    /// <summary>
    /// A string that, associated with <see cref="Tags"/>, uniquely identifies
    /// a metric.
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// A collection of key/value pairs that semantically identifies a
    /// metric.
    /// </summary>
    public IEnumerable<KeyValuePair<string, string>> Tags {
      get { return tags_; }
    }

    public override bool Equals(object obj) {
      if (obj == null) {
        return false;
      }

      return Equals(obj as MetricName);
    }

    public bool Equals(MetricName obj) {
      if ((object) obj == null) {
        return false;
      }

      if (obj.Key == Key) {
        foreach (var tag in tags_) {
          string obj_tag;
          if (!(obj.tags_.TryGetValue(tag.Key, out obj_tag)
            && tag.Value == obj_tag)) {
            return false;
          }
        }
        return true;
      }
      return false;
    }

    int ComputeHashCode() {
      unchecked {
        int hash = 17;
        hash = hash*23 + tags_.GetHashCode();
        hash = hash*23 + Key.GetHashCode();
        return hash;
      }
    }

    public override int GetHashCode() {
      return hashcode_;
    }
  }
}
