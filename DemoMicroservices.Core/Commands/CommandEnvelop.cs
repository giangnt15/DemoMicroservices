using DemoMicroservices.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Commands
{
    public class CommandEnvelop : Envelop
    {
        public string DestChannel { get; set; }
        public string ReplyChannel { get; set; }
    }
}
