using System;
using Nohros.Data;

namespace Nohros.Metrics.Data
{
  public abstract class StoreMetricCommand : IQuery
  {
    public abstract void Execute();

    public virtual string Name { get; set; }
    public virtual double Value { get; set; }
    public virtual long Timestamp { get; set; }
  }
}
