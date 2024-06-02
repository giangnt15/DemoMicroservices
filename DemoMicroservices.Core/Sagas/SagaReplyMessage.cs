using DemoMicroservices.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Sagas
{
    public class SagaReplyMessage : Message, IMessage
    {
    }

    public class PseudoSagaReplyMessage : SagaReplyMessage
    {
    }
}
