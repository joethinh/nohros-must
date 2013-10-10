using System;

namespace Nohros.Data.SqlServer
{
  class HiLoRangeDto : IHiLoRange
  {
    public long High { get; set; }
    public long MaxLow { get; set; }
  }
}
