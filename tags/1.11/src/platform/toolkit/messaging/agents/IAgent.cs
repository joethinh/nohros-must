using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// Defines the sender or recipient of a message.
  /// </summary>
  public interface IAgent
  {
    /// <summary>
    /// Gets or sets a string that represents the agent's name.
    /// </summary>
    /// <remarks>An agent is primarly identified by its address. The agent
    /// name is only a friendly form of agent identification. Two agent object
    /// with the same name does not nescessarly indicate that they are the same
    /// agent.</remarks>
    string Name { get; set; }

    /// <summary>
    /// Gets a string that uniquely identifies an agent.
    /// </summary>
    string Address { get; }
  }
}