using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Toolkit.Messaging;

namespace Nohros.Test.Toolkit.Messaging
{
    public class SimpleMessenger : IMessenger
    {
        IDictionary<string, string> options_;
        string name_;

        #region .ctor
        public SimpleMessenger(string name, IDictionary<string, string> options) {
            name_ = name;
            options_ = options;
        }
        #endregion

        public ResponseMessage Send(IMessage message) {
            return new ResponseMessage(ResponseMessageType.ProcessedMessage);
        }

        public string GetRequiredOption(string name) {
            return Messenger.GetRequiredOption(name, options_);
        }

        public string GetRequiredOptionNull() {
            return Messenger.GetRequiredOption(name_, null);
        }

        public IDictionary<string, string> Options {
            get { return options_; }
        }

        public string Name {
            get { return name_; }
            set { name_ = value; }
        }
    }
}
