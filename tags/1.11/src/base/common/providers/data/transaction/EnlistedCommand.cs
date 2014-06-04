using System;
using System.Data;

namespace Nohros.Data.Providers
{
  class EnlistedCommand
  {
    readonly ExecuteCommandDelegate executor_;
    readonly PrepareCommandDelegate preparing_;

    #region .ctor
    public EnlistedCommand(PrepareCommandDelegate preparing,
      ExecuteCommandDelegate executor) {
      executor_ = executor;
      preparing_ = preparing;
    }
    #endregion

    public PrepareCommandDelegate Prepare {
      get { return preparing_; }
    }

    public ExecuteCommandDelegate Execute {
      get { return executor_; }
    }
  }
}
