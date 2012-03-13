using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nohros.MessageQueue.RabbitMQ
{
  public class RabbitMessageMiddleware: IMessageMiddleware<RabbitMessage>
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMessageMiddleware"/>
    /// class by using the specified
    /// </summary>
    public RabbitMessageMiddleware() {
    }
    #endregion

    bool IMessageMiddleware.Enqueue(IMessage message, IOperationData parms) {
      // process only rabbitmq messages
      RabbitMessage m = message as RabbitMessage;
      if (m == null) {
        return false;
      }
      return Enqueue(m, parms);
    }

    IMessage IMessageMiddleware.Dequeue(IOperationData parms) {
      return Dequeue(parms) as IMessage;
    }

    public bool Enqueue(RabbitMessage message, IOperationData parms) {
    }

    public RabbitMessage Dequeue(IOperationData parms) {
    }
  }
}
