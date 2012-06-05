﻿using System;

namespace Nohros.Toolkit.RestQL
{
  public interface IQueryServer
  {
    /// <summary>
    /// Gets the server's query processor.
    /// </summary>
    IQueryProcessor QueryProcessor { get; }

    /// <summary>
    /// Gets the server's setting.
    /// </summary>
    ISettings Settings { get; }

    /// <summary>
    /// Gets the server's principal mapper.
    /// </summary>
    ITokenPrincipalMapper TokenPrincipalMapper { get; }
  }
}
