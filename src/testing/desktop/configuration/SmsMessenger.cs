using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Toolkit.Messaging;

namespace Nohros.Test.Configuration
{
    public class SmsMessenger : Messenger
    {
        #region .ctor
        public SmsMessenger(string name):base(name) { }
        #endregion

        public override void ProcessResponse(IMessage message) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override IMessage Send(IMessage message) {
            return new ResponseMessage("response");
        }
    }
}