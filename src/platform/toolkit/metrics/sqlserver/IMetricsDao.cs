﻿using System;
using System.Collections.Generic;

namespace Nohros.Metrics.Sql
{
  public interface IMetricsDao
  {
    IEnumerable<long> GetTagsIds(int hash, int count);

    long RegisterTags(int hash, int count);

    void RegisterTag(string name, string value, long tags_id);

    bool ContainsTag(string name, string value, long tags_id);

    void RegisterMeasure(long tags_id, double value, DateTime timestamp);
  }
}
